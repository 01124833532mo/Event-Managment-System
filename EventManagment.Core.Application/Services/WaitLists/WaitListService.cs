using AutoMapper;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Services.WaitLists;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Waitlists;
using EventManagment.Core.Domain.Specifications.WalitLists;
using EventManagment.Shared.Models.WaitList;

namespace EventManagment.Core.Application.Services.WaitLists
{
    public class WaitListService(IUnitOfWork unitOfWork, IMapper mapper) : IWaitListService
    {
        public async Task<Pagination<WaitListToReturn>> GetWalitListAsync(SpecParams spec, CancellationToken cancellationToken = default)
        {
            var waitListSpec = new WaitListPaginatedSpecification(spec.PageSize, spec.PageIndex);
            var walitlistrepo = unitOfWork.GetRepository<WaitList, int>();
            var waitLists = await walitlistrepo.GetAllWithSpecAsync(waitListSpec);
            var countspec = new waitlistCountSpecification();
            var count = await walitlistrepo.GetCountAsync(countspec, cancellationToken);
            var data = mapper.Map<IEnumerable<WaitListToReturn>>(waitLists);
            return new Pagination<WaitListToReturn>(spec.PageIndex, spec.PageSize, count) { Data = data };


        }
    }
}
