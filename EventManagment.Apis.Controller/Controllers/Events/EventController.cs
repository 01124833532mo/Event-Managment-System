using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Events;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Events
{




    [Authorize(Roles = Roles.Organizer + "," + Roles.Admin)]
    public class EventController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("CreateEvent")]
        public async Task<ActionResult> CreateEvent([FromBody] EventDto eventDto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventServices.CreateEvent(eventDto, cancellationToken);
            return NewResult(result);

        }

        [HttpPut("UpdateEvent/{id}")]
        public async Task<ActionResult> UpdateEvent([FromRoute] int id, [FromBody] EventDto eventDto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventServices.UpdateEvent(id, eventDto, cancellationToken);
            return NewResult(result);

        }

        [HttpPut("CancelEvent/{id}")]
        public async Task<ActionResult> CancelEvent([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventServices.CancelEvent(id, cancellationToken);
            return NewResult(result);

        }

        [HttpDelete("DeleteEvent/{id}")]
        public async Task<ActionResult> DeleteEvent([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventServices.DeleteEvent(id, cancellationToken);
            return NewResult(result);

        }
        [AllowAnonymous]
        [HttpGet("GetEventById/{id}")]
        public async Task<ActionResult> GetEventById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.EventServices.GetEventByIdAsync(id, cancellationToken);
            return NewResult(result);

        }
        [AllowAnonymous]
        [HttpGet("GetAllEvents")]
        public async Task<ActionResult<Pagination<EventToreturn>>> GetAllEvents([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.EventServices.GetAllEventsAsynce(specParams, cancellationToken);
            return Ok(products);
        }
    }
}
