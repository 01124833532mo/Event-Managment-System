using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Speakers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Speakers
{
    [Authorize]
    public class SpeakerController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet("Speakers")]
        public async Task<ActionResult<Pagination<SpeakerToReturn>>> GetAllSpeakers([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.SpeakerService.GetAllSpeakersAsync(specParams, cancellationToken);
            return Ok(products);
        }
    }
}
