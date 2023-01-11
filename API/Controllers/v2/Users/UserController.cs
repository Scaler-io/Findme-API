using API.Extensions;
using API.Filters;
using API.Models.Constants;
using API.Models.Requests.User;
using API.Services.Interfaces.v2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace API.Controllers.v2.Users
{
    [Authorize]
    [ApiVersion("2")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(ILogger logger, IIdentityService identityService, IUserService userService) 
            : base(logger, identityService)
        {
            _userService = userService;
        }

        [HttpGet("user")]
        [AccessPermission(ApiAccess.GenericRole)]
        public async Task<IActionResult> GetUsers()
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.GetUsers();
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        [HttpGet("user/{id}")]
        [AccessPermission(ApiAccess.GenericRole)]
        public async Task<IActionResult> GetUser([FromRoute]int id)
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.GetUserById(id);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        [HttpPut("user")]
        [AccessPermission(ApiAccess.GenericRole)]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserUpdateRequest request)
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.UpdateUserInfo(request, CurrentUser);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }
    }
}