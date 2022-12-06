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
using ILogger = Serilog.ILogger;

namespace API.Controllers.v2.Account
{
    [ApiVersion("2")]
    public class LoginController: BaseApiController
    {
        private readonly IValidator<UserLoginRequest> _validator;
        private readonly IIdentityService _identityService;

        public LoginController(ILogger logger, IValidator<UserLoginRequest> validator, 
            IIdentityService identityService)
            : base(logger)
        {
            _validator = validator;
            _identityService = identityService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthSuccessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            Logger.Here().MethoEnterd();
            var validationResult = IsValidRequest(request);
            if (IsInvalidResult(validationResult)) return ProcessValidationResult(validationResult);
            var result = await _identityService.Login(request);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        private ValidationResult IsValidRequest(UserLoginRequest request)
        {
            Logger.Here().Information("Request user login {@request}", request);
            var validationResult = _validator.Validate(request);
            if (!IsInvalidResult(validationResult)) return validationResult;
            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCodes.UnprocessableEntity, request);
            return validationResult;
        }
    }
}
