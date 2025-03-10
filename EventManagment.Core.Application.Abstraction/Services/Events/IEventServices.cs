using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Events;

namespace EventManagment.Core.Application.Abstraction.Services.Events
{
    public interface IEventServices
    {
        public Task<Response<EventToreturn>> CreateEvent(EventDto eventDto);

        public Task<Response<EventToreturn>> UpdateEvent(int id, EventDto eventDto);

        Task<Response<string>> DeleteEvent(int id);


        Task<Response<EventToreturn>> GetEventById(int id, CancellationToken cancellationToken);

    }
}
