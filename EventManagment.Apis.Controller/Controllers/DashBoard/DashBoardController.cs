using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Shared.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.DashBoard
{
    public class DashBoardController(IServiceManager serviceManager) : BaseApiController
    {

        [HttpGet("GetRoles")]
        public async Task<ActionResult> GetRoles()
        {
            var result = await serviceManager.AuthService.GetRolesAsync();
            return Ok(result);
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(RoleDtoBase roleDto)
        {
            var result = await serviceManager.AuthService.CreateRoleAsync(roleDto);
            return Ok(result);

        }
        [HttpDelete("DeleteRole/{id}")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            await serviceManager.AuthService.DeleteRole(id);
            return Ok("Delete Successfully");
        }
        [HttpPut("UpdateRole/{id}")]
        public async Task<ActionResult> UpdateRole(string id, RoleDtoBase roleDto)
        {
            var result = await serviceManager.AuthService.UpdateRole(id, roleDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetUsers")]
        public async Task<ActionResult> GetUsers()
        {
            var result = await serviceManager.AuthService.GetAllAttendences();
            return Ok(result);
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToReturn>> CreateUser(CreateAttendenceDro createUserDro)
        {
            var result = await serviceManager.AuthService.CreateAttendences(createUserDro);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<AttendentRoleViewModel>> GetUser([FromRoute] string id)
        {
            var result = await serviceManager.AuthService.GetAttendence(id);
            return Ok(result);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult<string>> DeleteUser([FromRoute] string id)
        {
            var result = await serviceManager.AuthService.DeleteAttendence(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetTechnicals")]
        public async Task<ActionResult> GetOrganizers()
        {
            var result = await serviceManager.AuthService.GetAllOrganizers();
            return Ok(result);
        }

    }
}
