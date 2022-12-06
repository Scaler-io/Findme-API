using API.Extensions;
using API.Models.Constants;
using API.Models.Core;
using API.Models.Requests.Account;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.v2.Account
{
    [ApiVersion("2")]
    public class RegistrationController : BaseApiController
    {
        private readonly IIdentityService _identityService;
        private readonly IValidator<UserRegistrationRequest> _validtor;

        public RegistrationController(Serilog.ILogger logger,
            IIdentityService identityService, IValidator<UserRegistrationRequest> validtor) : base(logger)
        {
            _identityService = identityService;
            _validtor = validtor;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthSuccessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            Logger.Here().MethoEnterd();

            var validationResult = IsValidRequest(request);
            if (IsInvalidResult(validationResult)) return ProcessValidationResult(validationResult);

            var result = await _identityService.Register(request);

            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        private ValidationResult IsValidRequest(UserRegistrationRequest request)
        {
            Logger.Here().Information("Request user registration {@request}", request);
            var validationResult = _validtor.Validate(request);
            if(!IsInvalidResult(validationResult)) return validationResult;

            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCodes.UnprocessableEntity, request);
            return validationResult;
        } 

        private static bool IsInvalidResult(ValidationResult validationResult)
        {
            return validationResult != null && !validationResult.IsValid;
        }
    }
}
