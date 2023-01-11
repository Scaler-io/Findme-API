using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Requests.Photo;
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
    public class PhotoDeleteController: BaseApiController
    {
        private readonly IValidator<PhotoDeleteRequest> _validator;
        private readonly IPhotoUploadService _photoUploadService;

        public PhotoDeleteController(Serilog.ILogger logger, IIdentityService identityService
            , IPhotoUploadService photoUploadService, IValidator<PhotoDeleteRequest> validator)
            : base(logger, identityService)
        {
            _photoUploadService = photoUploadService;
            _validator = validator;
        }

        [HttpDelete("photo")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeletePhoto([FromQuery] PhotoDeleteRequest request)
        {
            Logger.Here().MethoEnterd();
            var validationResult = IsValidRequest(request);
            if (IsInvalidResult(validationResult)) return ProcessValidationResult(validationResult);
            var result = await _photoUploadService.DeletePhoto(request);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        private ValidationResult IsValidRequest(PhotoDeleteRequest request)
        {
            Logger.Here().Information("Request photo delete {@request}", request);
            var validationResult = _validator.Validate(request);
            if (!IsInvalidResult(validationResult)) return validationResult;
            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCodes.UnprocessableEntity, request);
            return validationResult;
        }
    }
}
