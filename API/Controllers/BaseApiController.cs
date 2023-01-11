using API.Models.Constants;
using API.Models.Core;
using API.Services.Interfaces.v2;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace API.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    public class BaseApiController: ControllerBase
    {
        public ILogger Logger { get; set; }

        public IIdentityService _identityService { get; set; }
        public BaseApiController(ILogger logger, IIdentityService identityService)
        {
            _identityService = identityService;
            Logger = logger;
        }
        protected UserDto CurrentUser => _identityService.GetCurrentUser();




        public IActionResult OkOrFail<T>(Result<T> result){
            if(result == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if(result.IsSuccess && result.Value == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            switch (result.ErrorCode)
            {
                case ErrorCodes.NotFound:
                    return NotFound(new ApiResponse(ErrorCodes.NotFound, result.ErrorMessage ?? ErrorMessages.NotFound));
                case ErrorCodes.Unauthorized:
                    return Unauthorized(new ApiResponse(ErrorCodes.Unauthorized, result.ErrorMessage ?? ErrorMessages.Unauthorized));
                case ErrorCodes.Operationfailed:
                    return BadRequest(new ApiResponse(ErrorCodes.Operationfailed, ErrorMessages.Operationfailed ?? ErrorMessages.Operationfailed));
                default:
                    return BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage ?? ErrorMessages.BadRequest));
            }
        }

        public IActionResult CreatedWithRoute<T>(Result<T> result, string routeName, object param)
        {
            if (result.IsSuccess && result.Value != null) return CreatedAtRoute(
                    routeName,
                    param,
                    result.Value
                );

            return OkOrFail(result);
        }

        protected IActionResult ProcessValidationResult(ValidationResult validationResult)
        {
            validationResult.AddToModelState(ModelState);
            var errors = ModelState.Where(err => err.Value.Errors.Count > 0).ToList();
            var validationError = new ApiValidationResponse()
            {
                Errors = new List<FieldLevelError>()
            };
            foreach (var error in errors)
            {
                Logger.Information(JsonConvert.SerializeObject(error));
                var fieldLevelError = new FieldLevelError
                {
                    Code = "Invalid",
                    Field = error.Key,
                    Message = error.Value.Errors?.First().ErrorMessage
                };
                validationError.Errors.Add(fieldLevelError);
            }
            return new UnprocessableEntityObjectResult(validationError);
        }

        public static bool IsInvalidResult(ValidationResult validationResult)
        {
            return validationResult != null && !validationResult.IsValid;
        }
    }
}