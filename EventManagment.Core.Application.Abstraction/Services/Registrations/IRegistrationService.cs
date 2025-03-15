using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Registrations;

namespace EventManagment.Core.Application.Abstraction.Services.Registrations
{
    public interface IRegistrationService
    {
        Task<Response<RegisterToReturn>> CreateRegisterAsync(CreateRegisterDto createRegisterDto, CancellationToken cancellationToken);

    }
}
