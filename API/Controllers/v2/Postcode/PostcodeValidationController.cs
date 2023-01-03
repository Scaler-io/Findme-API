using API.Extensions;
using API.Models.Core;
using API.Models.Requests.Postcode;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.v2.Postcode
{
    [ApiVersion("2")]
    public class PostcodeValidationController : BaseApiController
    {
        private readonly IPostcodeValidationService _postcodeValidationService;

        public PostcodeValidationController(Serilog.ILogger logger, IIdentityService identityService, IPostcodeValidationService postcodeValidationService)
            : base(logger, identityService)
        {
            _postcodeValidationService = postcodeValidationService;
        }

        [HttpGet("postcode/{postcode}")]
        [ProducesResponseType(typeof(PostcodeValidationResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ValidatePostcod([FromRoute] PostcodeSearchRequest request)
        {
            Logger.Here().MethoEnterd();
            var result = await _postcodeValidationService.ValidatePostcodeAsync(request);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }
    }
}
