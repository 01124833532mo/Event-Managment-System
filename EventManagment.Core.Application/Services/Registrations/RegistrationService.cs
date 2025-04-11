using AutoMapper;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Core.Application.Abstraction.Services.Emails;
using EventManagment.Core.Application.Abstraction.Services.Registrations;
using EventManagment.Core.Application.Abstraction.Services.WaitLists;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Core.Domain.Enums;
using EventManagment.Core.Domain.Specifications.Registrations;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models._Common.Emails;
using EventManagment.Shared.Models.Registrations;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EventManagment.Core.Application.Services.Registrations
{
    public class RegistrationService(IUnitOfWork _unitOfWork
        , IMapper _mapper,
        ILogger<RegistrationService> _logger,
        UserManager<ApplicationUser> userManager,
        IPaymentService paymentService,
        IEmailService emailService,
        ILoggedInUserService loggedInUserService,
        IWaitListService waitListService) : ResponseHandler, IRegistrationService
    {


        public async Task<Pagination<RegisterToReturn>> GetAllRegistrationsAsync(SpecParams specParams, CancellationToken cancellationToken = default)
        {
            var spec = new RegistrationWithEventAndCategorySpecification(specParams.Sort, specParams.EventId, specParams.RegistrationId, specParams.PageSize, specParams.PageIndex);

            _logger.LogInformation("Get All Registrations Service Called");

            var registrations = await _unitOfWork.GetRepository<Registration, int>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IEnumerable<RegisterToReturn>>(registrations);
            var countSpec = new RegistrationWithFilterationForCountSpecifications(specParams.EventId, specParams.RegistrationId);
            var count = await _unitOfWork.GetRepository<Registration, int>().GetCountAsync(countSpec, cancellationToken);

            return new Pagination<RegisterToReturn>(specParams.PageIndex, specParams.PageSize, count) { Data = data };
        }


        public async Task<Pagination<RegisterToReturn>> GetAllRegistrationForSpecificUserAsync(SpecParams specParams, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
        {
            var spec = new RegistrationWithEventAndCategorySpecification(specParams.Sort, specParams.EventId, specParams.RegistrationId, specParams.PageSize, specParams.PageIndex);
            var registrations = await _unitOfWork.GetRepository<Registration, int>().GetAllWithSpecAsync(spec);

            var attendeeid = claimsPrincipal.FindFirstValue(ClaimTypes.PrimarySid);
            var specificregistration = registrations.Where(p => p.AttendeeId.Equals(attendeeid)).ToList();
            var data = _mapper.Map<IEnumerable<RegisterToReturn>>(specificregistration);

            var count = specificregistration.Count;

            return new Pagination<RegisterToReturn>(specParams.PageIndex, specParams.PageSize, count) { Data = data };

        }

        public async Task<Response<RegisterToReturn>> GetRegistrationAsync(int id, CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.GetRepository<Registration, int>();
            _logger.LogInformation("Get Registration By Id Service Called");
            var spec = new RegistrationWithEventAndCategorySpecification(id);

            var registration = await repo.GetWithSpecAsync(spec, cancellationToken);
            if (registration is null)
            {
                _logger.LogWarning("Registration Not Found With This Id");
                return NotFound<RegisterToReturn>(id, "Registration Not Found With This Id");
            };
            var mappedRegister = _mapper.Map<RegisterToReturn>(registration);
            _logger.LogInformation("Event Found And Mapped");


            return Success(mappedRegister, 1);
        }

        public async Task<Response<RegisterToReturn>> CreateRegisterAsync(CreateRegisterDto createRegisterDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("CreateRegisterAsync called");

            var checkeventexsist = await _unitOfWork.GetRepository<Event, int>().GetAsync(createRegisterDto.Eventid, cancellationToken);
            if (checkeventexsist is null) return NotFound<RegisterToReturn>(createRegisterDto.Eventid, "Event Not Exsist with This Id");

            else if (checkeventexsist.Data < DateTime.Now) return BadRequest<RegisterToReturn>("Event Date is Passed");

            else if (checkeventexsist.Status == EventStatus.canceled) return BadRequest<RegisterToReturn>("Event is Canceled");
            else if (checkeventexsist.Status == EventStatus.completed) return BadRequest<RegisterToReturn>("Event is Completed");

            else if (checkeventexsist.MaxAttendees <= checkeventexsist.Registrations.Count)
            {
                var attendeeid = loggedInUserService.UserId;
                var waitlist = await waitListService.AddToWaitListAsync(createRegisterDto.Eventid, attendeeid!, cancellationToken);
                if (waitlist.Succeeded)
                {

                    RecurringJob.AddOrUpdate(
                                  $"WaitListCheck_{createRegisterDto.Eventid}_{attendeeid}",
                                  () => CheckForAvailableSpotAsync(createRegisterDto.Eventid, attendeeid!, cancellationToken),
                                  "*/5 * * * *"
                              );

                    return BadRequest<RegisterToReturn>(
                        "Event is full. You have been added to the waitlist. " +
                        "We will notify you if a spot becomes available.");
                }
                else if (!waitlist.Succeeded)
                {
                    return BadRequest<RegisterToReturn>("Event is Full, Error Occured While Adding To Wait List");
                }

            }

            var repo = _unitOfWork.GetRepository<Registration, int>();
            var register = _mapper.Map<Registration>(createRegisterDto);
            try
            {
                await repo.AddAsync(register);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateRegisterAsync failed");
                return BadRequest<RegisterToReturn>("Failed to create register");
            }

            var complete = await _unitOfWork.CompleteAsync() > 0;

            if (!complete)
            {
                _logger.LogWarning("CreateRegisterAsync failed");
                return BadRequest<RegisterToReturn>("Failed to create register");
            }

            var Attendee = await userManager.FindByIdAsync(register.AttendeeId) as Attendde;


            if (Attendee!.WaitLists?.Any(x => !x.IsNotified && x.EventId == createRegisterDto.Eventid) ?? false)
            {
                var unnotifiedEntries = Attendee.WaitLists
                    .Where(x => !x.IsNotified && x.EventId == createRegisterDto.Eventid)
                    .ToList();

                foreach (var entry in unnotifiedEntries)
                {
                    entry.IsNotified = true;

                }
                var chekcomplete = await _unitOfWork.CompleteAsync() > 0;
                if (!chekcomplete)
                {
                    _logger.LogWarning("CreateRegisterAsync failed");
                    return BadRequest<RegisterToReturn>("Failed to create register");
                }



            }

            if (Attendee is null)
            {
                throw new BadRequestExeption("User not found");
            }
            var regiserid = register.Id;

            var result = await paymentService.CreateOrUpdatePaymentIntent(regiserid, cancellationToken);

            var returnedData = _mapper.Map<RegisterToReturn>(register);
            returnedData.FullName = Attendee.FullName;


            _logger.LogInformation("CreateRegisterAsync succeeded");

            _logger.LogInformation("Email sent to {0}", Attendee.Email);

            var EmailToResend = new Email()
            {
                Subject = "Registeration",
                Body = "Registeration You have successfully registered to the event",
                To = Attendee.Email!
            };
            BackgroundJob.Enqueue(() => emailService.SendEmail(EmailToResend));





            RecurringJob.AddOrUpdate(
                  $"EventNotification_{register.Id}", // id 
                  () => SendEventNotificationAsync(register.Id, checkeventexsist.Data, Attendee.Email!),
                  "0 0 */5 * *" // Every 5 days
              );

            return Success(returnedData);

        }
        public async Task SendEventNotificationAsync(int registerId, DateTime eventDate, string attendeeEmail)
        {
            if (DateTime.Now >= eventDate)
            {
                RecurringJob.RemoveIfExists($"EventNotification_{registerId}");
                _logger.LogInformation($"Recurring job stopped for registration ID: {registerId}");
                return;
            }

            var date = eventDate - DateTime.Now;

            var NotificationMail = new Email()
            {
                Subject = "Event Notification",
                Body = $"Be CareFull, remaining for the event {date.Days} days, {date.Hours} hours, and {date.Minutes} minutes.",
                To = attendeeEmail
            };

            await emailService.SendEmail(NotificationMail);
        }

        public async Task<Response<string>> CancelRegistrationAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("CancelRegistrationAsync called");

            var repo = _unitOfWork.GetRepository<Registration, int>();

            var spec = new RegistrationWithEventAndCategorySpecification(id);

            var registration = await repo.GetWithSpecAsync(spec, cancellationToken);

            if (registration is null)
                return NotFound<string>(id, "Registration Not Found With This Id");


            repo.Delete(registration);

            await paymentService.CancelRegistrationAndRefund(registration.Id, cancellationToken);


            var complete = await _unitOfWork.CompleteAsync() > 0;

            if (!complete)
            {
                _logger.LogWarning("CancelRegistrationAsync failed");
                return BadRequest<string>("Failed to cancel register");
            }

            _logger.LogInformation("CancelRegistrationAsync succeeded");

            return Success("Registeration Canceled Successfully");


        }
        public async Task CheckForAvailableSpotAsync(int eventId, string attendeeId, CancellationToken cancellationToken = default)
        {
            var eventEntity = await _unitOfWork.GetRepository<Event, int>()
                .GetAsync(eventId, cancellationToken);

            if (eventEntity == null)
            {
                _logger.LogWarning($"Event {eventId} not found. Removing waitlist check job.");
                RecurringJob.RemoveIfExists($"WaitListCheck_{eventId}_{attendeeId}");
                return;
            }

            bool hasAvailableSpots = eventEntity.MaxAttendees > eventEntity.Registrations.Count;

            if (!hasAvailableSpots)
                return;


            var user = await userManager.FindByIdAsync(attendeeId);
            if (user?.Email != null)
            {
                var email = new Email
                {
                    Subject = "🚀 A Spot Just Opened Up!",
                    Body = $"A spot is now available for event '{eventEntity.Title}'. " +
                           $"Hurry and register before it's gone!\n\n" +
                           $"Event Date: {eventEntity.Data.ToShortDateString()}",
                    To = user.Email
                };

                BackgroundJob.Enqueue(() => emailService.SendEmail(email));
            }

            RecurringJob.RemoveIfExists($"WaitListCheck_{eventId}_{attendeeId}");
        }


    }

}
