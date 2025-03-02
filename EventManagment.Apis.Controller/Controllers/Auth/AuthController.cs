using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Shared.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Auth
{

    public class AuthController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("login")]

        public async Task<ActionResult<UserToReturn>> Login(LoginDto model)
        {
            var result = await serviceManager.AuthService.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("Register")]

        public async Task<ActionResult<UserToReturn>> Register(RegisterDto model)
        {
            var result = await serviceManager.AuthService.RegisterAsync(model);
            return Ok(result);
        }
    }
}
