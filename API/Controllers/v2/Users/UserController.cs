using API.Extensions;
using API.Models.Requests.User;
using API.Services.Interfaces.v2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [AllowAnonymous]
        [HttpGet("user")]
        public async Task<IActionResult> GetUsers()
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.GetUsers();
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser([FromRoute]int id)
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.GetUserById(id);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserUpdateRequest request)
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.UpdateUserInfo(request, CurrentUser);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }
    }
}