using API.Extensions;
using API.Models.Core;
using API.Models.Responses;
using API.Services.Interfaces.v2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.v2.Account
{
    [ApiVersion("2")]
    [Authorize]
    public class AccountController : BaseApiController
    {
        public AccountController(Serilog.ILogger logger, IIdentityService identityService) 
            : base(logger, identityService)
        {
        }

        [HttpGet("{username}/usernameExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [AllowAnonymous]
        public async Task<IActionResult> IsUsernameExists([FromRoute] string username)
        {
            Logger.Here().MethoEnterd();
            var result = await _identityService.IsUsernameExist(username);
            Logger.Here().MethodExited();
            return Ok(result);
        }

        [HttpGet("account")]
        [ProducesResponseType(typeof(AuthSuccessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiValidationResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAuthenticatedUserDetails()
        {
            Logger.Here().MethoEnterd();
            var result = await _identityService.AutoLogin();
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }
    }
}
