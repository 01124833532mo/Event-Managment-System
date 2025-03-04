using EventManagment.Shared.Models._Common.Emails;
using EventManagment.Shared.Models.Auth;
using System.Security.Claims;

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

        Task<SuccessDto> ForgetPasswordByEmailasync(ForgetPasswordByEmailDto emailDto);
        Task<SuccessDto> VerifyCodeByEmailAsync(ResetCodeConfirmationByEmailDto resetCodeDto);
        Task<UserToReturn> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetCodeDto);

        Task<UserToReturn> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        Task<UserToReturn> GetCurrentUser(ClaimsPrincipal claimsPrincipal);

        Task<ChangePasswordToReturn> ChangePasswordAsync(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto);


    }
}
