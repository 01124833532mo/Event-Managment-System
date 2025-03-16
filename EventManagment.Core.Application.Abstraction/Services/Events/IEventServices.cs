using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Events;

namespace EventManagment.Core.Application.Abstraction.Services.Events
{
    public interface IEventServices
    {
        public Task<Response<EventToreturn>> CreateEvent(EventDto eventDto, CancellationToken cancellationToken);

        public Task<Response<EventToreturn>> UpdateEvent(int id, EventDto eventDto, CancellationToken cancellationToken);

        Task<Response<string>> DeleteEvent(int id, CancellationToken cancellationToken);


        Task<Response<EventToreturn>> GetEventByIdAsync(int id, CancellationToken cancellationToken);

        Task<Pagination<EventToreturn>> GetAllEventsAsynce(SpecParams specParams, CancellationToken cancellationToken);

        Task<Response<string>> CancelEvent(int id, CancellationToken cancellationToken);

    }
}
