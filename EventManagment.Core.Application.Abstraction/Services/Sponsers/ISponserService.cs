using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Sponsers;

namespace EventManagment.Core.Application.Abstraction.Services.Sponsers
{
    public interface ISponserService
    {
        public Task<Response<SponserToReturn>> CreateSponserAsync(CreateSponserDto sponserToReturn, CancellationToken cancellationToken = default);
        public Task<Response<string>> DeleteSponser(int id, CancellationToken cancellationToken);
        public Task<Pagination<SponserToReturn>> GetAllSponserAsync(SpecParams specParams, CancellationToken cancellationToken);
        public Task<Response<SponserToReturn>> GetSponserAsync(int id, CancellationToken cancellationToken);


    }
}
