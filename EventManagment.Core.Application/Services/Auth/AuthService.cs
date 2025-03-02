using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Domain._Identity;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models.Auth;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagment.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager
        , IOptions<JwtSettings> jwtsettings) : IAuthService
    {
        private readonly JwtSettings _jwtsettings = jwtsettings.Value;
        public async Task<UserToReturn> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                throw new UnAuthorizedExeption("Invalid Login");

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);


            if (result.IsLockedOut)
                throw new UnAuthorizedExeption("Email is Locked Out");

            if (!result.Succeeded)
                throw new UnAuthorizedExeption("Invalid Login");

            var userroles = await userManager.GetRolesAsync(user);


            if (userroles.Any(role => role == Roles.Organizer))
            {
                var response = new UserToReturn
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber!,
                    Email = user.Email!,
                    Types = user.Types.ToString(),
                    Token = await GenerateTokenAsync(user),
                    RefreshToken = string.Empty, // No refresh token for organizer
                    RefreshTokenExpirationDate = DateTime.MinValue // No expiration date for organizer
                };
                return response;
            }
            else
            {
                var response = new UserToReturn
                {
                    Id = user.Id,
                    FullName = user.FullName!,
                    PhoneNumber = user.PhoneNumber!,
                    Email = user.Email!,
                    Types = user.Types.ToString(),
                    Token = await GenerateTokenAsync(user),
                    RefreshToken = string.Empty, // Initialize with empty string
                    RefreshTokenExpirationDate = DateTime.MinValue // Initialize with default value
                };
                //await CheckRefreshToken(userManager, user, response);

                return response;
            }
        }
        public async Task<UserToReturn> RegisterAsync(RegisterDto registerDto)
        {
            //if (EmailExists(model.Email).Result)
            //    throw new BadRequestExeption("This Email Is Already in User");
            var email = userManager.Users.Where(e => e.Email == registerDto.Email).FirstOrDefault();
            if (email is not null)
                throw new BadRequestExeption("Email Already Exsist");

            var user = new ApplicationUser()
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Types = registerDto.Types,


            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) throw new ValidationExeption() { Errors = result.Errors.Select(p => p.Description) };

            //var refresktoken = GenerateRefreshToken();

            //user.RefreshTokens.Add(new RefreshToken()
            //{
            //    Token = refresktoken.Token,
            //    ExpireOn = refresktoken.ExpireOn
            //});

            //await _userManager.UpdateAsync(user);



            var roleresult = registerDto.Types.ToString() == Roles.Attendee ? await userManager.AddToRoleAsync(user, Roles.Attendee.ToString())
                : await userManager.AddToRoleAsync(user, Roles.Organizer.ToString());

            if (!roleresult.Succeeded)
                throw new ValidationExeption() { Errors = roleresult.Errors.Select(E => E.Description) };

            var response = new UserToReturn()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Types = user.Types.ToString(),
                Token = await GenerateTokenAsync(user),

                RefreshToken = "refresktoken.Token",
                RefreshTokenExpirationDate = DateTime.Now,
            };

            return response;
        }



        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userclaims = await userManager.GetClaimsAsync(user);

            var userrolesclaims = new List<Claim>();

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                userrolesclaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            IEnumerable<Claim> claims;

            claims = new List<Claim>()
                {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber!),
                new Claim("Types",user.Types.ToString()),
                }
           .Union(userclaims)
           .Union(userrolesclaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(

                issuer: _jwtsettings.Issuer,
                audience: _jwtsettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtsettings.DurationInMinitutes),
                claims: claims,
                signingCredentials: signingCredentials
                );


            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }


    }
}
