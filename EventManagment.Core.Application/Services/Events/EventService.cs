using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Core.Application.Abstraction.Services.Emails;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.Sponsers;
using EventManagment.Core.Domain.Enums;
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
    public class EventService(IUnitOfWork _unitOfWork,
        IMapper _mapper, UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ILogger<EventService> logger,
        IPaymentService paymentService) : ResponseHandler, IEventServices
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
            var organizername = await userManager.FindByIdAsync(Event.OrganizerId);
            if (organizername == null)
            {
                throw new BadRequestExeption("Organizer not found");
            }
            var mappedEvent = _mapper.Map<EventToreturn>(Event);
            mappedEvent.OrganizerName = organizername.FullName;
            logger.LogInformation("Event Found And Mapped");


            return Success(mappedEvent, 1);




        }

        public async Task<Pagination<EventToreturn>> GetAllEventsAsynce(SpecParams specParams, CancellationToken cancellationToken = default)
        {
            var spec = new EvenstWithCategoryAndOrgnizerSpecification(specParams.Sort, specParams.CategoryId, specParams.Orgnizerid, specParams.PageSize, specParams.PageIndex, specParams.Search);


            var Events = await _unitOfWork.GetRepository<Event, int>().GetAllWithSpecAsync(spec);


            var data = _mapper.Map<IEnumerable<EventToreturn>>(Events);
            foreach (var item in data)
            {
                var organizername = await userManager.FindByIdAsync(item.OrganizerId);
                if (organizername == null)
                {
                    throw new BadRequestExeption("Organizer not found");
                }
                item.OrganizerName = organizername.FullName;
            }

            var countSpec = new EventWithFilterationForCountSpecifications(specParams.Orgnizerid, specParams.CategoryId, specParams.Search);
            var count = await _unitOfWork.GetRepository<Event, int>().GetCountAsync(countSpec, cancellationToken);

            return new Pagination<EventToreturn>(specParams.PageIndex, specParams.PageSize, count) { Data = data };
        }
        public async Task<Response<EventToreturn>> CreateEvent(EventDto eventDto, CancellationToken cancellationToken = default)
        {
            var sponser = await _unitOfWork.GetRepository<Sponser, int>().GetAsync(eventDto.SponserId, cancellationToken);
            if (sponser is null) return NotFound<EventToreturn>(eventDto.SponserId, "Sponser Not Exsist with This Id");

            var checkcategoryexsist = await _unitOfWork.GetRepository<Category, int>().GetAsync(eventDto.Categoryid, cancellationToken);
            if (checkcategoryexsist is null) return NotFound<EventToreturn>(eventDto.Categoryid, "Category Not Exsist with This Id");

            var mappedevent = _mapper.Map<Event>(eventDto);

            await _unitOfWork.GetRepository<Event, int>().AddAsync(mappedevent);


            var complete = await _unitOfWork.CompleteAsync() > 0;
            if (!complete) return BadRequest<EventToreturn>("Error Occure While Creating Event");
            var mappedresult = _mapper.Map<EventToreturn>(mappedevent);

            var FullNameUser = await userManager.FindByIdAsync(mappedresult.OrganizerId);

            if (FullNameUser == null)
            {
                throw new BadRequestExeption("User not found");
            }

            mappedresult.OrganizerName = FullNameUser.FullName;


            await SendEventEmailsToAttendees(userManager, logger, emailService, mappedresult);

            return Created(mappedresult);



        }


        public async Task<Response<EventToreturn>> UpdateEvent(int id, EventDto eventDto, CancellationToken cancellationToken = default)
        {

            var existingEvent = await _unitOfWork.GetRepository<Event, int>().GetAsync(id, cancellationToken);
            if (existingEvent == null)
                return NotFound<EventToreturn>(id, "Event not found with this ID");

            var checkCategoryExist = await _unitOfWork.GetRepository<Category, int>().GetAsync(eventDto.Categoryid, cancellationToken);
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

            await SendEventEmailsToAttendees(userManager, logger, emailService, mappedResult, isUpdate: true);


            // Return the updated event
            return Success(mappedResult);
        }

        public async Task<Response<string>> DeleteEvent(int id, CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.GetRepository<Event, int>();
            var Event = await repo.GetAsync(id, cancellationToken);

            if (Event is null) return NotFound<string>(id, "Not Event With This Id:"); ;

            repo.Delete(Event);


            var registration = Event.Registrations.ToList();
            foreach (var register in registration)
            {
                logger.LogInformation("Cancel Registration And Refund Service Called");
                await paymentService.CancelRegistrationAndRefund(register.Id, cancellationToken);

                var email = new Email()
                {
                    Subject = "Event Deleted",
                    Body = $"Dear {register.Attendee.FullName},\n\nThe event '{Event.Title}' has been Deleted.\n\nEvent Date: {Event.Data}  \n\n  Your Money Alaready Returned To Your Account \n\nThank you!",
                    To = register.Attendee.Email!
                };

            }

            var result = await _unitOfWork.CompleteAsync() > 0;

            if (result is true) return Deleted<string>();

            else
                return BadRequest<string>("Operation Faild");

        }

        public async Task<Response<string>> CancelEvent(int id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Cancel Event Service Called");

            var repo = _unitOfWork.GetRepository<Event, int>();
            var spec = new EventWithCategoryAndOrgnizerSpecification(id);
            var Event = await repo.GetWithSpecAsync(spec, cancellationToken);


            if (Event is null)
            {
                logger.LogWarning("Event Not Found With This Id");
                return NotFound<string>(id, "Event Not Found With This Id");
            };

            Event.Status = EventStatus.canceled!;
            var mappedResult = _mapper.Map<EventToreturn>(Event);
            repo.Update(Event);
            var result = await _unitOfWork.CompleteAsync() > 0;
            if (result is true)
            {
                await SendEventEmailsToAttendees(userManager, logger, emailService, mappedResult, isCancelled: true);

                logger.LogInformation("Event Canceled Successfully");
                return Success("Event Canceled Successfully");
            }
            else
            {
                logger.LogWarning("Error Occure While Canceling Event");
                return BadRequest<string>("Error Occure While Canceling Event");

            }
        }

        private async Task SendEventEmailsToAttendees(UserManager<ApplicationUser> userManager,
     ILogger logger,
     IEmailService emailService,
     EventToreturn mappedResult,
     bool isUpdate = false,
     bool isCancelled = false)
        {
            var allAttendees = await userManager.GetUsersInRoleAsync(Roles.Attendee);

            foreach (var attendee in allAttendees)
            {
                var emailSubject = isUpdate ? "Event Updated" :
                                 isCancelled ? "Event Cancelled" :
                                 "New Event Created";

                var emailMessage = isUpdate
                    ? $"Dear {attendee.FullName},\n\nA event Is Updated '{mappedResult.Title}' has been Updated Be Carfule For This Update.\n\nEvent Date: {mappedResult.Data}\n\nThank you!"
                    : isCancelled
                        ? $"Dear {attendee.FullName},\n\nThe event '{mappedResult.Title}' has been cancelled.\n\nEvent Date: {mappedResult.Data}\n\nThank you!"
                        : $"Dear {attendee.FullName},\n\nA new event '{mappedResult.Title}' has been created Do You Want To Register For This Event?.\n\nEvent Date: {mappedResult.Data}\n\nThank you!";

                var email = new Email()
                {
                    Subject = emailSubject,
                    Body = emailMessage,
                    To = attendee.Email!
                };

                logger.LogInformation("Email Send To Attendees");
                BackgroundJob.Enqueue(() => emailService.SendEmail(email));
            }
        }
    }
}
