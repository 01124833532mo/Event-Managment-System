using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.WaitList;

namespace EventManagment.Core.Application.Abstraction.Services.WaitLists
{
    public interface IWaitListService
    {
        public Task<Pagination<WaitListToReturn>> GetWalitListAsync(SpecParams spec, CancellationToken cancellationToken);

    }
}
