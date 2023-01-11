using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Requests.Photo;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.v2.Photo
{
    [ApiVersion("2")]
    [Authorize]
    public class PhotoController : BaseApiController
    {
        private readonly IValidator<PhotoUploadRequest> _validator;
        private readonly IValidator<PhotoDeleteRequest> _deletRequestValidator;
        private readonly IPhotoUploadService _photoUploadService;
        public PhotoController(Serilog.ILogger logger, IIdentityService identityService,
            IValidator<PhotoUploadRequest> photoUploadRequestValidator, IPhotoUploadService photoUploadService)
            : base(logger, identityService)
        {
            _validator = photoUploadRequestValidator;
            _photoUploadService = photoUploadService;
        }

        [HttpPost("photo/upload")]
        [ProducesResponseType(typeof(UserImageResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UploadPhoto([FromForm]PhotoUploadRequest request)
        {
            Logger.Here().MethoEnterd();
            var validationResult = IsValidRequest(request);
            if (IsInvalidResult(validationResult)) return ProcessValidationResult(validationResult);
            var result = await _photoUploadService.UploadPhoto(request);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        private ValidationResult IsValidRequest(PhotoUploadRequest request)
        {
            Logger.Here().Information("Request photo upload {@request}", request);
            var validationResult = _validator.Validate(request);
            if (!IsInvalidResult(validationResult)) return validationResult;
            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCodes.UnprocessableEntity, request);
            return validationResult;
        }
    }
}
