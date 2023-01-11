using API.DataAccess.Interfaces;
using API.DataAccess.Specifications.User;
using API.Entities;
using API.Extensions;
using API.Infrastructure.Cloudinary;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Requests.Photo;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using ILogger = Serilog.ILogger;

namespace API.Services.v2.Photo
{
    public class PhotoUploadService : IPhotoUploadService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger _logger;
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private IBaseRepository<AppUser> _userRepository;
        private readonly IMapper _mapper;

        public PhotoUploadService(IOptions<CloudinarySetting> cloudinarySettings,
            ILogger logger, IIdentityService identityService, IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            var account = new CloudinaryDotNet.Account
            {
                Cloud = cloudinarySettings.Value.CloudName,
                ApiKey = cloudinarySettings.Value.ApiKey,
                ApiSecret = cloudinarySettings.Value.ApiSecret
            };

            _cloudinary = new Cloudinary(account);
            _logger = logger;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.Repository<AppUser>();
            _mapper = mapper;
        }

        public async Task<Result<bool>> DeletePhoto(PhotoDeleteRequest request)
        {
            _logger.Here().MethoEnterd();
            var spec = new FindUserByUserNameSpec(_identityService.GetCurrentUser().Username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            var photo = user.Profile.Photos.FirstOrDefault(x => x.PublicId == request.PublicId);
            if(photo == null)
            {
                _logger.Here().Information("No photo was found with id {@PublicId}.", request.PublicId);
                return Result<bool>.Fail(ErrorCodes.NotFound);
            }

            if (photo.IsMain)
            {
                _logger.Here().Information($"{ErrorCodes.BadRequest}: Trying to delete main photo.");
                return Result<bool>.Fail(ErrorCodes.BadRequest, "Trying to delete main photo.");
            }

            var deleteParams = new DeletionParams(request.PublicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            if(result.Error != null)
            {
                _logger.Here().Error("{0} Image deletion failed. {1}", ErrorCodes.InternalServerError, result.Error);
                return Result<bool>.Fail(ErrorCodes.InternalServerError);
            }

            user.Profile.Photos.Remove(photo);

            if(await _unitOfWork.Complete() < 1)
            {
                _logger.Here().Error("{0} Photo deleteion from database failed.", ErrorCodes.InternalServerError);
                return Result<bool>.Fail(ErrorCodes.InternalServerError, "Photo deletion from database failed.");
            }

            _logger.Here().Information("Photo deleted successfully");
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        public async Task<Result<UserImageResponse>> UploadPhoto(PhotoUploadRequest request)
        {
            _logger.Here().MethoEnterd();
            var file = request.File;
            var uploadResult = new ImageUploadResult();
            var uploadParams = new ImageUploadParams();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                uploadParams = PrepareImageUploadParams(file, stream);
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            if(uploadResult.Error != null)
            {
                _logger.Here().Error("Image upload to cloudinary failed for {0}, [Error:{1}]", 
                    _identityService.GetCurrentUser().Username, uploadResult.Error.Message);
                return Result<UserImageResponse>.Fail(ErrorCodes.InternalServerError, "Image upload failed");
            }

            _logger.Here().Information("Image upload to cloudinary success. {@publicId}", uploadParams.PublicId);
            _logger.Here().MethodExited();
            return await HandleImageUpladResponse(uploadResult);
        }
        
        private ImageUploadParams PrepareImageUploadParams(IFormFile file, Stream stream)
        {          
            var newFileName = $"{file.FileName}_{DateTime.UtcNow}";
            return new ImageUploadParams
            {
                File = new FileDescription(newFileName, stream),
                Transformation = new Transformation()
                                    .Height(500)
                                    .Width(500)
                                    .Crop("fill")
                                    .Gravity("face"),
                Folder = _identityService.GetCurrentUser().Username
            };
        }
    
        private async Task<Result<UserImageResponse>> HandleImageUpladResponse(ImageUploadResult result)
        {
            var spec = new FindUserByUserNameSpec(_identityService.GetCurrentUser().Username);
            var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(spec);

            var userImage = new UserImage
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if(user.Profile.Photos.Count() == 0)
            {
                userImage.IsMain = true;
            }
            else
            {
                userImage.IsMain = false; 
            }
            user.Profile.Photos.Add(userImage);
            _unitOfWork.Repository<UserImage>().Add(userImage);

            if(await _unitOfWork.Complete() != 1)
            {
                _logger.Here().Error($"{ErrorCodes.InternalServerError}: Updating database with new image entry failed");
                return Result<UserImageResponse>.Fail(ErrorCodes.InternalServerError, ErrorMessages.InternalServerError);
            }

            _logger.Here().Error("Updating database with new image entry success. {@publicId}", userImage.PublicId);
            var response = _mapper.Map<UserImageResponse>(userImage);
            return Result<UserImageResponse>.Success(response);
        }

        public async Task<Result<UserImageResponse>> UpdatePhotoAsMain(PhotoUpdateRequest request)
        {
            _logger.Here().MethoEnterd();

            var spec = new FindUserByUserNameSpec(_identityService.GetCurrentUser().Username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            var photos = user.Profile.Photos;

            if(photos.Count() == 0)
            {
                _logger.Here().Error("[ErrorCode:{0}] No photos found for the user", ErrorCodes.NotFound);
                return Result<UserImageResponse>.Fail(ErrorCodes.NotFound, "No photos found for the user");
            }

            var selectedPhoto = photos.FirstOrDefault(x => x.PublicId == request.PublicId);
            if(selectedPhoto == null)
            {
                _logger.Here().Error("[ErrorCode:{0}] No photos found with public id {1}", ErrorCodes.BadRequest, request);
                return Result<UserImageResponse>.Fail(ErrorCodes.BadRequest);
            }

            if (selectedPhoto.IsMain)
            {
                _logger.Here().Information("{0} Photo {1} already set to main", ErrorCodes.Operationfailed, request.PublicId);
                return Result<UserImageResponse>.Fail(ErrorCodes.Operationfailed);
            }

            var currentMainPhoto = user.Profile.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
                selectedPhoto.IsMain = true;
            }

            if(await _unitOfWork.Complete() < 1)
            {
                _logger.Here().Information("{0}: Falied to update photo with id. {1}",
                    ErrorCodes.InternalServerError, request.PublicId);
                return Result<UserImageResponse>.Fail(ErrorCodes.InternalServerError, "Failed to update photo.");
            }

            _logger.Here().Information("The photo has been set as main. {0}", request.PublicId);
            _logger.Here().MethodExited();

            var result = _mapper.Map<UserImageResponse>(selectedPhoto);
            return Result<UserImageResponse>.Success(result);
        }
    }
}
