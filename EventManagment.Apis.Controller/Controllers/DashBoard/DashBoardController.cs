using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Shared.Models.Auth;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.DashBoard
{
    [Authorize(Roles = Roles.Admin)]

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

        [HttpGet("GetAttendes")]
        public async Task<ActionResult> GetUsers()
        {
            var result = await serviceManager.AuthService.GetAllAttendences();
            return Ok(result);
        }

        [HttpPost("CreateAttende")]
        public async Task<ActionResult<BaseToReturn>> CreateUser(CreateAttendenceDro createUserDro)
        {
            var result = await serviceManager.AuthService.CreateAttendences(createUserDro);
            return Ok(result);
        }
        [HttpGet("GetAttende/{id}")]
        public async Task<ActionResult<AttendentRoleViewModel>> GetUser([FromRoute] string id)
        {
            var result = await serviceManager.AuthService.GetAttendence(id);
            return Ok(result);
        }

        [HttpDelete("DeleteAttende/{id}")]
        public async Task<ActionResult<string>> DeleteUser([FromRoute] string id)
        {
            var result = await serviceManager.AuthService.DeleteAttendence(id);
            return Ok(result);
        }

        [HttpGet("GetOrganizers")]
        public async Task<ActionResult> GetOrganizers()
        {
            var result = await serviceManager.AuthService.GetAllOrganizers();
            return Ok(result);
        }

    }
}
