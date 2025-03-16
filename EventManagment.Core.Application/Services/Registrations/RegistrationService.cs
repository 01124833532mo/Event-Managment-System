using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Core.Application.Abstraction.Services.Emails;
using EventManagment.Core.Application.Abstraction.Services.Registrations;
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
        IEmailService emailService) : ResponseHandler, IRegistrationService
    {


        public async Task<Pagination<RegisterToReturn>> GetAllRegistrationsAsync(SpecParams specParams, CancellationToken cancellationToken = default)
        {
            var spec = new RegistrationWithEventAndCategorySpecification(specParams.Sort, specParams.EventId, specParams.RegistrationId, specParams.PageSize, specParams.PageIndex);


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

            var checkcategoryexsist = await _unitOfWork.GetRepository<Event, int>().GetAsync(createRegisterDto.Eventid);
            if (checkcategoryexsist is null) return NotFound<RegisterToReturn>(createRegisterDto.Eventid, "Event Not Exsist with This Id");

            else if (checkcategoryexsist.Data < DateTime.Now) return BadRequest<RegisterToReturn>("Event Date is Passed");

            else if (checkcategoryexsist.Status == EventStatus.canceled) return BadRequest<RegisterToReturn>("Event is Canceled");
            else if (checkcategoryexsist.Status == EventStatus.completed) return BadRequest<RegisterToReturn>("Event is Completed");

            else if (checkcategoryexsist.MaxAttendees <= checkcategoryexsist.Registrations.Count) return BadRequest<RegisterToReturn>("Event is Full");

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

            var Attendee = await userManager.FindByIdAsync(register.AttendeeId);

            if (Attendee is null)
            {
                throw new BadRequestExeption("User not found");
            }
            var regiserid = register.Id;

            var result = await paymentService.CreateOrUpdatePaymentIntent(regiserid);

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
                  () => SendEventNotificationAsync(register.Id, checkcategoryexsist.Data, Attendee.Email!),
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
    }
}
