using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models.Events;
using Microsoft.AspNetCore.Identity;

namespace EventManagment.Core.Application.Services.Events
{
    public class EventService(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<ApplicationUser> userManager) : ResponseHandler, IEventServices
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

            return Created(mappedresult);



        }
    }
}
