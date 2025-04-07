using AutoMapper;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Services.Speakers;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Speakers;
using EventManagment.Core.Domain.Specifications.Speakers;
using EventManagment.Shared.Models.Speakers;

namespace EventManagment.Core.Application.Services.Speakers
{
    public class SpeakersService(IUnitOfWork unitOfWork, IMapper mapper) : ISpeakerService
    {
        public async Task<Pagination<SpeakerToReturn>> GetAllSpeakersAsync(SpecParams specParams, CancellationToken cancellationToken = default)
        {
            var spec = new GetAllSpeakersSpecification(specParams.PageSize, specParams.PageIndex);

            var speakers = await unitOfWork.GetRepository<Speaker, int>().GetAllWithSpecAsync(spec);

            var count = speakers.Count();

            var data = mapper.Map<IEnumerable<SpeakerToReturn>>(speakers);


            return new Pagination<SpeakerToReturn>(specParams.PageIndex, specParams.PageSize, count) { Data = data };

        }
    }
}
