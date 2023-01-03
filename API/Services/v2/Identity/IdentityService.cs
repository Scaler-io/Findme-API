using API.DataAccess.Interfaces;
using API.DataAccess.Specifications.User;
using API.Entities;
using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Enums;
using API.Models.Requests.Account;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using ILogger = Serilog.ILogger;

namespace API.Services.v2.Account
{
    public class IdentityService : IIdentityService
    {
        private readonly ILogger _logger;
        private readonly IBaseRepository<AppUser> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.Repository<AppUser>();
            _mapper = mapper;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<AuthSuccessResponse>> Login(UserLoginRequest request)
        {
            _logger.Here().MethoEnterd();

            var spec = new FindUserByUserNameSpec(request.Username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            if (user == null)
            {
                _logger.Error("{ErrorCode} - Invalid credential given.", ErrorCodes.Unauthorized);
                return Result<AuthSuccessResponse>.Fail(ErrorCodes.Unauthorized, ErrorMessages.Unauthorized);
            }

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    _logger.Here().Error("{@ErrorCode} - Login attempt filed", ErrorCodes.Unauthorized);
                    return Result<AuthSuccessResponse>.Fail(ErrorCodes.Unauthorized, ErrorMessages.Unauthorized);
                }
            }

            await UpdateUserLoginTime(user);

            var result = HandleSuccessResponse(user);

            _logger.Here().Information("Login attempt successfull {@user}", result);
            _logger.Here().MethodExited();
            return Result<AuthSuccessResponse>.Success(result);
        }

        public async Task<Result<AuthSuccessResponse>> Register(UserRegistrationRequest request)
        {
            _logger.Here().MethoEnterd();

            var user = PopulateUser(request);
            
            // persistance
            _userRepository.Add(user);
            await _unitOfWork.Complete();

            var result = HandleSuccessResponse(user);

            _logger.Here().Information("User registered successfully {@user}", result);
            _logger.Here().MethodExited();
            return Result<AuthSuccessResponse>.Success(result);
        }

        private AuthSuccessResponse HandleSuccessResponse(AppUser user)
        {
            var response = _mapper.Map<AuthSuccessResponse>(user);
            response.Token = _tokenService.CreateToken(user);

            return response;
        }

        private AppUser PopulateUser(UserRegistrationRequest request)
        {
            DateTime.TryParse(request.Profile.DateOfBirth, out var dob);
            Enum.TryParse<Gender>(request.Profile.Gender, out var gender);

            using var hmac = new HMACSHA512();
            return new AppUser
            {
                UserName = request.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key,
                Profile = new UserProfile
                {
                    FirstName = request.Profile.FirstName,
                    LastName = request.Profile.LastName,
                    DateOfBirth = dob,
                    KnownAs = request.Profile.KnownAs,
                    Gender = gender,
                    Address = new UserAddress
                    {
                        StreetNumber = request.Address.StreetNumber,
                        StreetName = request.Address.StreetName,
                        StreetType = request.Address.StreetType,
                        City = request.Address.City,
                        District = request.Address.District,
                        State = request.Address.State,
                        PostCode = request.Address.PostCode
                    },
                    CreatedBy = request.Username
                },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                LastLogin = null
            };
        }
    
        private async Task UpdateUserLoginTime(AppUser user)
        {
            _logger.Here().MethoEnterd();
            user.LastLogin = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _unitOfWork.Complete();

            _logger.Here().Information("User details updated");
            _logger.Here().MethodExited();
        }

        public UserDto GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = user.GetAuthUsername();
            var spec = new FindUserByUserNameSpec(userId);
            var userResult = _userRepository.GetEntityWithSpec(spec).Result;
            return _mapper.Map<UserDto>(userResult);
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            _logger.Here().MethoEnterd();
            var spec = new FindUserByUserNameSpec(username);
            var user = await _userRepository.GetEntityWithSpec(spec);
            if(user != null)
            {
                _logger.Here().Warning("{@ErrorCode} username already exists.");
                return true;
            }

            _logger.Here().MethodExited();
            return false;
        }

        public async Task<Result<AuthSuccessResponse>> AutoLogin()
        {
            _logger.Here().MethoEnterd();

            var spec = new FindUserByUserNameSpec(GetCurrentUser().Username);
            var user = await _userRepository.GetEntityWithSpec(spec);

            var response = HandleSuccessResponse(user);

            _logger.Here().MethodExited();
            return Result<AuthSuccessResponse>.Success(response);
        }
    }
}
