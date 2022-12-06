using API.Extensions;
using API.Services.Interfaces.v2;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace API.Controllers.v2.Users
{
    [ApiVersion("2")]
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(ILogger logger, IUserService userService) : base(logger)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.GetUsers();
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute]int id)
        {
            Logger.Here().MethoEnterd();
            var result = await _userService.GetUserById(id);
            Logger.Here().MethodExited();
            return OkOrFail(result);
        }
    }
}