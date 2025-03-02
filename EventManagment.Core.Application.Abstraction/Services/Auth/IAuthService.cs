using EventManagment.Shared.Models.Auth;

namespace EventManagment.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserToReturn> LoginAsync(LoginDto loginDto);

        Task<UserToReturn> RegisterAsync(RegisterDto registerDto);


    }
}
