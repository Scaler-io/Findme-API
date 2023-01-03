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
        private readonly IValidator<UserRegistrationRequest> _validator;

        public RegistrationController(Serilog.ILogger logger,
            IIdentityService identityService, IValidator<UserRegistrationRequest> validtor) 
            : base(logger, identityService)
        {
            _identityService = identityService;
            _validator = validtor;
        }

        [HttpPost("registration")]
        [ProducesResponseType(typeof(AuthSuccessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            Logger.Here().MethoEnterd();

            var validationResult = IsValidRequest(request);
            if (IsInvalidResult(validationResult)) return ProcessValidationResult(validationResult);

            if (await _identityService.IsUsernameExist(request.Username))
            {
                Logger.Here().Warning("{@ErrorCode}: Registration failed. username already taken. {@username}",
                    ErrorCodes.Operationfailed, request.Username);
                return CreateDuplicateUsernameResposne(request.Username);
            }

            var result = await _identityService.Register(request);

            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        private ValidationResult IsValidRequest(UserRegistrationRequest request)
        {
            Logger.Here().Information("Request user registration {@request}", request);
            var validationResult = _validator.Validate(request);
            if(!IsInvalidResult(validationResult)) return validationResult;

            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCodes.UnprocessableEntity, request);
            return validationResult;
        }

        private IActionResult CreateDuplicateUsernameResposne(string username)
        {
            return UnprocessableEntity(new ApiValidationResponse
            {
                Code = ErrorCodes.Operationfailed,
                Errors = new List<FieldLevelError> {
                    new FieldLevelError
                    {
                        Code = ErrorCodes.UnprocessableEntity,
                        Field = "username",
                        Message = $"Username '{username}' is already taken."
                    }
                }
            });
        }
    }
}
