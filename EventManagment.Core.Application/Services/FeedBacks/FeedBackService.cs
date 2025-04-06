using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Services.FeedBacks;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.FeedBacks;
using EventManagment.Core.Domain.Specifications.FeedBacks;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models.FeedBacks;
using System.Security.Claims;

namespace EventManagment.Core.Application.Services.FeedBacks
{
    public class FeedBackService(IUnitOfWork unitOfWork, IMapper mapper) : ResponseHandler, IFeedBackService
    {

        public async Task<Pagination<FeedBackToRetuen>> GetAllFeedBack(SpecParams specParams, CancellationToken cancellationToken = default)
        {

            var Event = await unitOfWork.GetRepository<Event, int>().GetAsync(specParams.EventId ?? 0, cancellationToken);
            if (Event is null)
                throw new NotFoundExeption("Event Not Found With This Id", specParams.EventId!);

            var spec = new GetwithEventAndAttenddeSpecification(specParams.EventId, specParams.Attendeeid, specParams.PageSize, specParams.PageIndex);

            var FeedBacks = await unitOfWork.GetRepository<Feedback, int>().GetAllWithSpecAsync(spec);
            var data = mapper.Map<IEnumerable<FeedBackToRetuen>>(FeedBacks);
            var countSpec = new FeedBackWithFilterationForCountSpecifications(specParams.EventId, specParams.Attendeeid);
            var count = await unitOfWork.GetRepository<Feedback, int>().GetCountAsync(countSpec, cancellationToken);
            return new Pagination<FeedBackToRetuen>(specParams.PageIndex, specParams.PageSize, count) { Data = data };

        }




        public async Task<Response<string>> CreateFeedBack(ClaimsPrincipal claims, CreateFeedBackDto createFeedBackDto, CancellationToken cancellationToken = default)
        {

            {

                var repo = unitOfWork.GetRepository<Event, int>();

                if (claims is null)
                    return Unauthorized<string>("You are not authorized to rate this event");




                var Attendeeid = claims.FindFirstValue(ClaimTypes.PrimarySid);

                if (Attendeeid is null)
                    return Unauthorized<string>("You are not authorized to rate this event");




                var Event = await repo.GetAsync(createFeedBackDto.EventId, cancellationToken);
                if (Event is null)
                    return NotFound<string>(createFeedBackDto.EventId, "Event not found");

                var FeedBack = new Feedback()
                {
                    Rate = createFeedBackDto.Rate,
                    Comment = createFeedBackDto.Comment,
                    EventId = createFeedBackDto.EventId,
                    AttenddeId = Attendeeid
                };

                try
                {
                    await unitOfWork.GetRepository<Feedback, int>().AddAsync(FeedBack);
                }
                catch (Exception ex)
                {
                    return BadRequest<string>(ex.Message);
                }

                Event.EventRate = (Event.EventRate * Event.EventCountRating + createFeedBackDto.Rate) / (Event.EventCountRating + 1);
                Event.EventCountRating += 1;



                var result = await unitOfWork.CompleteAsync() > 0;
                if (!result)
                    return BadRequest<string>("Failed to submit FeedBack");

                return Success("Succssfully Add FeedBack");

            }
        }



        public async Task<Response<string>> RemoveFeedBack(int id, CancellationToken cancellationToken)
        {
            var repo = unitOfWork.GetRepository<Feedback, int>();

            var FeedBack = await repo.GetAsync(id, cancellationToken);
            if (FeedBack is null)
                return NotFound<string>(id, "FeedBack not found");

            try
            {
                repo.Delete(FeedBack);
            }
            catch (Exception ex)
            {
                return BadRequest<string>(ex.Message);
            }

            var result = await unitOfWork.CompleteAsync() > 0;
            if (!result)
                return BadRequest<string>("Failed to remove FeedBack");

            return Success("Succssfully Remove FeedBack");
        }
    }
}
