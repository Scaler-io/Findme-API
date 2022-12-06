using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace API.Controllers.v1
{
    [ApiVersion("1")]
    public class HealthcheckController : BaseApiController
    {
        public HealthcheckController(ILogger logger) : base(logger)
        {
        }

        [HttpGet] 
        public IActionResult GetApiHealthStatus()
        {
            return Ok("Api status - Healthy");
        }
    }
}
