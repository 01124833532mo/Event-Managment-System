using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Services.Speakers;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Speakers;
using EventManagment.Core.Domain.Specifications.Speakers;
using EventManagment.Shared.Models.Speakers;

namespace EventManagment.Core.Application.Services.Speakers
{
    public class SpeakersService(IUnitOfWork unitOfWork, IMapper mapper) : ResponseHandler, ISpeakerService
    {
        public async Task<Pagination<SpeakerToReturn>> GetAllSpeakersAsync(SpecParams specParams, CancellationToken cancellationToken = default)
        {
            var spec = new GetAllSpeakersSpecification(specParams.PageSize, specParams.PageIndex);

            var speakers = await unitOfWork.GetRepository<Speaker, int>().GetAllWithSpecAsync(spec);

            var count = speakers.Count();

            var data = mapper.Map<IEnumerable<SpeakerToReturn>>(speakers);


            return new Pagination<SpeakerToReturn>(specParams.PageIndex, specParams.PageSize, count) { Data = data };

        }

        public async Task<Response<SpeakerToReturn>> GetSpeakerAsync(int id, CancellationToken cancellationToken = default)
        {

            var spec = new GetAllSpeakersSpecification(id);

            var speaker = await unitOfWork.GetRepository<Speaker, int>().GetWithSpecAsync(spec, cancellationToken);

            if (speaker is null)
                return NotFound<SpeakerToReturn>($"Speaker with id {id} not found");

            var mappedspeaker = mapper.Map<SpeakerToReturn>(speaker);

            return Success(mappedspeaker, 1);


        }
    }
}
