using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Events;

namespace EventManagment.Core.Application.Abstraction.Services.Events
{
    public interface IEventServices
    {
        public Task<Response<EventToreturn>> CreateEvent(EventDto eventDto);

    }
}
