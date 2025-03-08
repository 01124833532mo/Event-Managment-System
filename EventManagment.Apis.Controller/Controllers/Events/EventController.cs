using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Shared.Models.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Events
{
    [Authorize]
    public class EventController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("CreateEvent")]
        public async Task<ActionResult> CreateEvent([FromBody] EventDto eventDto)
        {
            var result = await serviceManager.EventServices.CreateEvent(eventDto);
            return NewResult(result);

        }

        [HttpPut("UpdateEvent/{id}")]
        public async Task<ActionResult> UpdateEvent([FromRoute] int id, [FromBody] EventDto eventDto)
        {
            var result = await serviceManager.EventServices.UpdateEvent(id, eventDto);
            return NewResult(result);

        }

        [HttpDelete("DeleteEvent/{id}")]
        public async Task<ActionResult> DeleteEvent([FromRoute] int id)
        {
            var result = await serviceManager.EventServices.DeleteEvent(id);
            return NewResult(result);

        }
    }
}
