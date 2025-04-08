using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Services.Sesstions;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.Sessions;
using EventManagment.Core.Domain.Entities.Speakers;
using EventManagment.Core.Domain.Specifications.Sessions;
using EventManagment.Shared.Models.Sesstions;
using Microsoft.Extensions.Configuration;

namespace EventManagment.Core.Application.Services.Sesstions
{
    public class SesstionService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : ResponseHandler, ISesstionService
    {
        public async Task<Response<SesstionToreturn>> CreateSesstionAsync(SesstionDto sesstionDto, CancellationToken cancellationToken)
        {
            var Speaker = await unitOfWork.GetRepository<Speaker, int>().GetAsync(sesstionDto.SpeakerId, cancellationToken);
            if (Speaker is null)
            {
                return NotFound<SesstionToreturn>($"Speaker not found with {sesstionDto.SpeakerId}");
            }
            var Event = await unitOfWork.GetRepository<Event, int>().GetAsync(sesstionDto.EventId, cancellationToken);
            if (Event is null)
            {
                return NotFound<SesstionToreturn>($"Event not found with {sesstionDto.EventId}");
            }
            var sesstion = mapper.Map<Session>(sesstionDto);

            try
            {
                await unitOfWork.GetRepository<Session, int>().AddAsync(sesstion);
            }
            catch (Exception ex)
            {
                return BadRequest<SesstionToreturn>(ex.Message);
            }
            var complete = await unitOfWork.CompleteAsync() > 0;
            if (!complete)
            {
                return BadRequest<SesstionToreturn>("Failed to create sesstion");
            }

            var mappedSesstion = mapper.Map<SesstionToreturn>(sesstion);
            mappedSesstion.SpeakerPhotoUrl = configuration["Urls:ApiBaseUrl"] + "/" + Speaker.PhotoUrl;
            return Success(mappedSesstion);

        }



        public async Task<Response<SesstionToreturn>> GetSesstionAsync(int id, CancellationToken cancellationToken)
        {
            var spec = new GetAllSesstionSpecification(id);
            var sesstion = await unitOfWork.GetRepository<Session, int>().GetWithSpecAsync(spec, cancellationToken);
            if (sesstion is null)
            {
                return NotFound<SesstionToreturn>($"Sesstion not found with {id}");
            }
            var mappedSesstion = mapper.Map<SesstionToreturn>(sesstion);
            mappedSesstion.SpeakerPhotoUrl = configuration["Urls:ApiBaseUrl"] + "/" + sesstion.Speaker!.PhotoUrl;

            return Success(mappedSesstion, 1);
        }

        public async Task<Response<string>> DeleteSesstionAsync(int id, CancellationToken cancellationToken)
        {
            var spec = new GetAllSesstionSpecification(id);
            var sesstionrepository = unitOfWork.GetRepository<Session, int>();

            var sesstion = await sesstionrepository.GetWithSpecAsync(spec, cancellationToken);

            if (sesstion is null)
                return NotFound<string>($"Sesstion with id {id} not found");
            try
            {

                sesstionrepository.Delete(sesstion);
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);

            }

            var result = unitOfWork.CompleteAsync().Result > 0;

            if (!result)
                return BadRequest<string>("Failed to delete sesstion");

            return Success("Deleted successfully", 1);
        }
    }
}
