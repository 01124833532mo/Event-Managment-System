using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Roles;
using EventManagment.Shared.Models.Sesstions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Session
{
    [Authorize(Roles = Roles.Admin)]

    public class SessionController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("CreateSession")]
        public async Task<ActionResult<Response<SesstionToreturn>>> CreateSession([FromBody] SesstionDto sesstionDto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.SesstionService.CreateSesstionAsync(sesstionDto, cancellationToken);
            return NewResult(result);

        }


    }
}
