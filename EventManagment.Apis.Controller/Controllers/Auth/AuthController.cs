using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Shared.Models._Common.Emails;
using EventManagment.Shared.Models.Auth;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("ForgetPasswordEmail")]
        public async Task<ActionResult> ForgetPasswordEmail(ForgetPasswordByEmailDto forgetPasswordDto)
        {
            var result = await serviceManager.AuthService.ForgetPasswordByEmailasync(forgetPasswordDto);
            return Ok(result);
        }
        [HttpPost("VerfiyCodeEmail")]
        public async Task<ActionResult> VerfiyCodeEmail(ResetCodeConfirmationByEmailDto resetCode)
        {
            var result = await serviceManager.AuthService.VerifyCodeByEmailAsync(resetCode);
            return Ok(result);
        }


        [HttpPut("ResetPasswordEmail")]
        public async Task<ActionResult> ResetPasswordEmail(ResetPasswordByEmailDto resetPassword)
        {
            var result = await serviceManager.AuthService.ResetPasswordByEmailAsync(resetPassword);
            return Ok(result);
        }
        [HttpPost("Get-Refresh-Token")]

        public async Task<ActionResult<UserToReturn>> RefreshToken([FromBody] RefreshDto model)
        {
            var result = await serviceManager.AuthService.GetRefreshToken(model);
            return Ok(result);
        }

        [HttpPost("Revoke-Refresh-Token")]
        public async Task<ActionResult> RevokeRefreshToken([FromBody] RefreshDto model)
        {
            var result = await serviceManager.AuthService.RevokeRefreshTokenAsync(model);
            return result is false ? BadRequest("Operation Not Successed") : Ok(result);

        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserToReturn>> GetCurrentUser()
        {

            var result = await serviceManager.AuthService.GetCurrentUser(User);
            return Ok(result);
        }
        [Authorize]

        [HttpPost("Change-Password")]
        public async Task<ActionResult<ChangePasswordDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await serviceManager.AuthService.ChangePasswordAsync(User, changePasswordDto);
            return Ok(result);
        }
    }
}
