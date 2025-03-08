using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Services.Emails;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models._Common.Emails;
using EventManagment.Shared.Models.Events;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Identity;

namespace EventManagment.Core.Application.Services.Events
{
    public class EventService(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<ApplicationUser> userManager, IEmailService emailService) : ResponseHandler, IEventServices
    {
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
                var emailMessage = $"Dear {attendee.FullName},\n\nA new event '{mappedresult.CategoryName}' has been created Do You Want To Register For This Event?.\n\nEvent Date: {mappedresult.Data}\n\nThank you!";
                var email = new Email()
                {
                    Subject = emailSubject,
                    Body = emailMessage,
                    To = attendee.Email!
                };
                await emailService.SendEmail(email);
            }

            return Created(mappedresult);



        }
    }
}
