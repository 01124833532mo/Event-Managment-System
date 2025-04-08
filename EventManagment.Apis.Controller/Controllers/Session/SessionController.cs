using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Roles;
using EventManagment.Shared.Models.Sesstions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Session
{
    [Authorize]
    public class SessionController(IServiceManager serviceManager) : BaseApiController
    {
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("CreateSession")]
        public async Task<ActionResult<Response<SesstionToreturn>>> CreateSession([FromBody] SesstionDto sesstionDto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.SesstionService.CreateSesstionAsync(sesstionDto, cancellationToken);
            return NewResult(result);

        }

        [HttpGet("GetSession/{id}")]
        public async Task<ActionResult<Response<SesstionToreturn>>> GetSession([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.SesstionService.GetSesstionAsync(id, cancellationToken);
            return NewResult(result);

        }


    }
}
