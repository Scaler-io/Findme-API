using API.Models.Constants;
using API.Models.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    [Route("api/v{version:apiVersion]/[controller]")]
    [ApiController]
    public class BaseApiController: ControllerBase
    {
        public ILogger Logger { get; set; }
        public BaseApiController(ILogger logger)
        {
            Logger = logger;
        }

        public IActionResult OkOrFail<T>(Result<T> result){
            if(result == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if(result.IsSuccess && result.Value == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            switch (result.ErrorCode)
            {
                case ErrorCodes.NotFound:
                    return NotFound(new ApiResponse(ErrorCodes.NotFound, result.ErrorMessage));
                case ErrorCodes.Unauthorized:
                    return Unauthorized(new ApiResponse(ErrorCodes.Unauthorized, result.ErrorMessage));
                case ErrorCodes.Operationfailed:
                    return BadRequest(new ApiResponse(ErrorCodes.Operationfailed, ErrorMessages.Operationfailed));
                default:
                    return BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage));
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
    }
}