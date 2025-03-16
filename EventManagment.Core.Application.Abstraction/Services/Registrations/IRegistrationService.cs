using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Registrations;
using System.Security.Claims;

namespace EventManagment.Core.Application.Abstraction.Services.Registrations
{
    public interface IRegistrationService
    {
        Task<Response<RegisterToReturn>> CreateRegisterAsync(CreateRegisterDto createRegisterDto, CancellationToken cancellationToken);

        Task<Response<RegisterToReturn>> GetRegistrationAsync(int id, CancellationToken cancellationToken);

        Task<Pagination<RegisterToReturn>> GetAllRegistrationsAsync(SpecParams specParams, CancellationToken cancellationToken);

        Task<Pagination<RegisterToReturn>> GetAllRegistrationForSpecificUserAsync(SpecParams specParams, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);

        Task<Response<string>> CancelRegistrationAsync(int id, CancellationToken cancellationToken);


    }
}
