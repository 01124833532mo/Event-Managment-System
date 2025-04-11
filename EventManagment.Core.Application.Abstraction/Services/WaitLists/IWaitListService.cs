using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.WaitList;

namespace EventManagment.Core.Application.Abstraction.Services.WaitLists
{
    public interface IWaitListService
    {
        public Task<Pagination<WaitListToReturn>> GetWalitListAsync(SpecParams spec, CancellationToken cancellationToken);
        public Task<Response<WaitListToReturn>> GetWaitListByIdAsync(int id, CancellationToken cancellationToken);

        public Task<Response<bool>> AddToWaitListAsync(int eventId, string attendeeId, CancellationToken cancellationToken);
    }
}
