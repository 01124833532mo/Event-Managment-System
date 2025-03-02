using EventManagment.Shared.Models.Auth;

namespace EventManagment.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserToReturn> LoginAsync(LoginDto loginDto);

        Task<UserToReturn> RegisterAsync(RegisterDto registerDto);


        Task<IEnumerable<RolesToReturn>> GetRolesAsync();

        Task<RolesToReturn> CreateRoleAsync(RoleDtoBase roleDto);

        Task DeleteRole(string id);
        Task<RolesToReturn> UpdateRole(string id, RoleDtoBase roleDto);

        Task<IEnumerable<AttendencesViewModel>> GetAllAttendences();
        Task<UserToReturn> CreateAttendences(CreateAttendenceDro createUserDro);

        Task<AttendentRoleViewModel> GetAttendence(string id);

        Task<string> DeleteAttendence(string id);
        Task<IEnumerable<OrganizerViewModel>> GetAllOrganizers();


    }
}
