using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Services.Emails;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Specifications.Events;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models._Common.Emails;
using EventManagment.Shared.Models.Events;
using EventManagment.Shared.Models.Roles;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventManagment.Core.Application.Services.Events
{
    public class EventService(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<ApplicationUser> userManager, IEmailService emailService, ILogger<EventService> logger) : ResponseHandler, IEventServices
    {

        public async Task<Response<EventToreturn>> GetEventByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.GetRepository<Event, int>();
            logger.LogInformation("Get Event By Id Service Called");
            var spec = new EventWithCategoryAndOrgnizerSpecification(id);

            var Event = await repo.GetWithSpecAsync(spec, cancellationToken);
            if (Event is null)
            {
                logger.LogWarning("Event Not Found With This Id");
                return NotFound<EventToreturn>(id, "Event Not Found With This Id");
            };
            var mappedEvent = _mapper.Map<EventToreturn>(Event);
            logger.LogInformation("Event Found And Mapped");


            return Success(mappedEvent, 1);




        }

        public async Task<Pagination<EventToreturn>> GetAllEventsAsynce(SpecParams specParams, CancellationToken cancellationToken)
        {
            var spec = new EvenstWithCategoryAndOrgnizerSpecification(specParams.Sort, specParams.CategoryId, specParams.Orgnizerid, specParams.PageSize, specParams.PageIndex, specParams.Search);


            var Events = await _unitOfWork.GetRepository<Event, int>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IEnumerable<EventToreturn>>(Events);
            var countSpec = new EventWithFilterationForCountSpecifications(specParams.Orgnizerid, specParams.CategoryId, specParams.Search);
            var count = await _unitOfWork.GetRepository<Event, int>().GetCountAsync(countSpec, cancellationToken);

            return new Pagination<EventToreturn>(specParams.PageIndex, specParams.PageSize, count) { Data = data };
        }
        public async Task<Response<EventToreturn>> CreateEvent(EventDto eventDto)
        {
            var checkcategoryexsist = await _unitOfWork.GetRepository<Category, int>().GetAsync(eventDto.Categoryid);
            if (checkcategoryexsist is null) return NotFound<EventToreturn>(eventDto.Categoryid, "Category Not Exsist with This Id");

            var mappedevent = _mapper.Map<Event>(eventDto);

            var addevent = _unitOfWork.GetRepository<Event, int>().AddAsync(mappedevent);
            if (addevent is null) return BadRequest<EventToreturn>("Operation No Successfuly");


            var complete = await _unitOfWork.CompleteAsync() > 0;
            if (!complete) return BadRequest<EventToreturn>("Error Occure While Creating Event");
            var mappedresult = _mapper.Map<EventToreturn>(mappedevent);

            var FullNameUser = await userManager.FindByIdAsync(mappedresult.OrganizerId);

            if (FullNameUser == null)
            {
                throw new BadRequestExeption("User not found");
            }

            mappedresult.OrganizerName = FullNameUser.FullName;


            var allattendees = await userManager.GetUsersInRoleAsync(Roles.Attendee);
            foreach (var attendee in allattendees)
            {
                var emailSubject = "New Event Created";
                var emailMessage = $"Dear {attendee.FullName},\n\nA new event '{mappedresult.Title}' has been created Do You Want To Register For This Event?.\n\nEvent Date: {mappedresult.Data}\n\nThank you!";
                var email = new Email()
                {
                    Subject = emailSubject,
                    Body = emailMessage,
                    To = attendee.Email!
                };
                //await emailService.SendEmail(email);
                logger.LogInformation("Email Send To Attendees");
                BackgroundJob.Enqueue(() => emailService.SendEmail(email));
            }

            return Created(mappedresult);



        }


        public async Task<Response<EventToreturn>> UpdateEvent(int id, EventDto eventDto)
        {

            var existingEvent = await _unitOfWork.GetRepository<Event, int>().GetAsync(id);
            if (existingEvent == null)
                return NotFound<EventToreturn>(id, "Event not found with this ID");

            var checkCategoryExist = await _unitOfWork.GetRepository<Category, int>().GetAsync(eventDto.Categoryid);
            if (checkCategoryExist == null)
                return NotFound<EventToreturn>(eventDto.Categoryid, "Category does not exist with this ID");

            var updatedEvent = _mapper.Map(eventDto, existingEvent);

            _unitOfWork.GetRepository<Event, int>().Update(existingEvent);


            var complete = await _unitOfWork.CompleteAsync() > 0;
            if (!complete)
                return BadRequest<EventToreturn>("Error occurred while updating the event");

            var mappedResult = _mapper.Map<EventToreturn>(updatedEvent);

            var organizer = await userManager.FindByIdAsync(mappedResult.OrganizerId);
            if (organizer == null)
            {
                throw new BadRequestExeption("organizer not found");
            }

            mappedResult.OrganizerName = organizer.FullName;

            var allattendees = await userManager.GetUsersInRoleAsync(Roles.Attendee);
            foreach (var attendee in allattendees)
            {
                var emailSubject = " Event Updated";
                var emailMessage = $"Dear {attendee.FullName},\n\nA  event Is Updated '{mappedResult.Title}' has been Updated Be Carfule For This Update.\n\nEvent Date: {mappedResult.Data}\n\nThank you!";
                var email = new Email()
                {
                    Subject = emailSubject,
                    Body = emailMessage,
                    To = attendee.Email!
                };
                logger.LogInformation("Email Send To Attendees");
                BackgroundJob.Enqueue(() => emailService.SendEmail(email));
            }


            // Return the updated event
            return Success(mappedResult);
        }

        public async Task<Response<string>> DeleteEvent(int id)
        {
            var repo = _unitOfWork.GetRepository<Event, int>();
            var Event = await repo.GetAsync(id);

            if (Event is null) return NotFound<string>(id, "Not Event With This Id:"); ;

            repo.Delete(Event);

            var result = await _unitOfWork.CompleteAsync() > 0;

            if (result is true) return Deleted<string>();

            else
                return BadRequest<string>("Operation Faild");

        }




    }
}
