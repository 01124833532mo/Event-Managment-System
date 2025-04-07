using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Speakers;

namespace EventManagment.Core.Application.Abstraction.Services.Speakers
{
    public interface ISpeakerService
    {
        public Task<Pagination<SpeakerToReturn>> GetAllSpeakersAsync(SpecParams specParams, CancellationToken cancellationToken);
    }
}
