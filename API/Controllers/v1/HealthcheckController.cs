using API.Services.Interfaces.v2;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace API.Controllers.v1
{
    [ApiVersion("1")]
    public class HealthcheckController : BaseApiController
    {
        public HealthcheckController(ILogger logger, IIdentityService identityService) 
            : base(logger, identityService)
        {
        }

        [HttpGet] 
        public IActionResult GetApiHealthStatus()
        {
            return Ok("Api status - Healthy");
        }
    }
}
