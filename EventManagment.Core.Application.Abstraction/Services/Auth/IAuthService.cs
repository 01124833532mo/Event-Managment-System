using EventManagment.Shared.Models._Common.Emails;
using EventManagment.Shared.Models.Auth;
using System.Security.Claims;

namespace EventManagment.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<BaseToReturn> LoginAsync(LoginDto loginDto);

        Task<BaseToReturn> RegisterAsync(RegisterDto registerDto);


        Task<IEnumerable<RolesToReturn>> GetRolesAsync();

        Task<RolesToReturn> CreateRoleAsync(RoleDtoBase roleDto);

        Task DeleteRole(string id);
        Task<RolesToReturn> UpdateRole(string id, RoleDtoBase roleDto);

        Task<IEnumerable<AttendencesViewModel>> GetAllAttendences();
        Task<BaseToReturn> CreateAttendences(CreateAttendenceDro createUserDro);

        Task<AttendentRoleViewModel> GetAttendence(string id);

        Task<string> DeleteAttendence(string id);
        Task<IEnumerable<OrganizerViewModel>> GetAllOrganizers();

        Task<SuccessDto> ForgetPasswordByEmailasync(ForgetPasswordByEmailDto emailDto);
        Task<SuccessDto> VerifyCodeByEmailAsync(ResetCodeConfirmationByEmailDto resetCodeDto);
        Task<BaseToReturn> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetCodeDto);

        Task<BaseToReturn> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        Task<BaseToReturn> GetCurrentUser(ClaimsPrincipal claimsPrincipal);

        Task<ChangePasswordToReturn> ChangePasswordAsync(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto);

        public Task<SuccessDto> ConfirmationCodeSendByEmailAsync(ForgetPasswordByEmailDto emailDto);
        public Task<SuccessDto> ConfirmEmailAsync(ConfirmationEmailCodeDto codeDto);


    }
}
