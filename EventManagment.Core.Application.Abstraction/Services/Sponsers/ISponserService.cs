using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Sponsers;

namespace EventManagment.Core.Application.Abstraction.Services.Sponsers
{
    public interface ISponserService
    {
        public Task<Response<SponserToReturn>> CreateSponserAsync(CreateSponserDto sponserToReturn, CancellationToken cancellationToken = default);
    }
}
