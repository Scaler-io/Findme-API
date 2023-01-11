using API.Extensions;
using API.Models.Constants;
using API.Models.Requests.Photo;
using API.Services.Interfaces.v2;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v2.Photo
{
    [ApiVersion("2")]
    [Authorize]
    public class PhotoUpdateController : BaseApiController
    {
        private readonly IValidator<PhotoUpdateRequest> _validator;
        private readonly IPhotoUploadService _photoUploadService;
        public PhotoUpdateController(Serilog.ILogger logger, IIdentityService identityService,
            IValidator<PhotoUpdateRequest> validator, IPhotoUploadService photoUploadService)
            : base(logger, identityService)
        {
            _validator = validator;
            _photoUploadService = photoUploadService;
        }

        [HttpPut("photo/update")]
        public async Task<IActionResult> UpdatePhotoAsMain([FromQuery]PhotoUpdateRequest request)
        {
            Logger.Here().MethoEnterd();
            var validationResult = IsValidRequest(request);
            if (IsInvalidResult(validationResult)) return ProcessValidationResult(validationResult);
            var result = await _photoUploadService.UpdatePhotoAsMain(request);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        private ValidationResult IsValidRequest(PhotoUpdateRequest request)
        {
            Logger.Here().Information("Request photo update {@request}", request);
            var validationResult = _validator.Validate(request);
            if (!IsInvalidResult(validationResult)) return validationResult;
            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCodes.UnprocessableEntity, request);
            return validationResult;
        }
    }
}
