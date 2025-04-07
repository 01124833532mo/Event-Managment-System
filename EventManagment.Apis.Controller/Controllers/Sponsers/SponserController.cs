using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Roles;
using EventManagment.Shared.Models.Sponsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Sponsers
{
    [Authorize(Roles = Roles.Admin)]
    public class SponserController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("CreateSponser")]
        public async Task<ActionResult<Response<SponserToReturn>>> CreateSponser([FromForm] CreateSponserDto createSponserDto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.SponserService.CreateSponserAsync(createSponserDto, cancellationToken);
            return NewResult(result);

        }

        [HttpDelete("DeleteSponser/{id}")]
        public async Task<ActionResult> DeleteSponser([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.SponserService.DeleteSponser(id, cancellationToken);
            return NewResult(result);
        }

    }
}
