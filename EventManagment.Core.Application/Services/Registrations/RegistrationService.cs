using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Core.Application.Abstraction.Services.Registrations;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Core.Domain.Specifications.Registrations;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models.Registrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EventManagment.Core.Application.Services.Registrations
{
    public class RegistrationService(IUnitOfWork _unitOfWork, IMapper _mapper, ILogger<RegistrationService> _logger, UserManager<ApplicationUser> userManager) : ResponseHandler, IRegistrationService
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

            var FullNameUser = await userManager.FindByIdAsync(register.AttendeeId);

            if (FullNameUser == null)
            {
                throw new BadRequestExeption("User not found");
            }
            var registerToReturn = _mapper.Map<RegisterToReturn>(register);
            registerToReturn.FullName = FullNameUser.FullName;

            return Success(registerToReturn);

        }


    }
}
