using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Domain._Identity;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models.Auth;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagment.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager
        , IOptions<JwtSettings> jwtsettings) : IAuthService
    {
        private readonly JwtSettings _jwtsettings = jwtsettings.Value;

        public async Task<RolesToReturn> CreateRoleAsync(RoleDtoBase roleDto)
        {
            var roleExsits = await roleManager.RoleExistsAsync(roleDto.Name);

            if (!roleExsits)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleDto.Name.Trim()));
                var role = await roleManager.FindByNameAsync(roleDto.Name);
                var mappedroleresult = new RolesToReturn() { Id = role!.Id, Name = role.Name! };
                return mappedroleresult;
            }
            else
            {
                throw new BadRequestExeption("This Role already Exists");
            }


        }

        public async Task<RolesToReturn> UpdateRole(string id, RoleDtoBase roleDto)
        {
            var roleExsists = await roleManager.RoleExistsAsync(roleDto.Name);
            if (!roleExsists)
            {
                var role = await roleManager.FindByIdAsync(id);
                role!.Name = roleDto.Name;
                await roleManager.UpdateAsync(role);
                var result = new RolesToReturn() { Id = role!.Id, Name = role.Name! };
                return result;
            }
            else
            {
                throw new BadRequestExeption("this Role Already is Exsists");
            }
        }
        public async Task<IEnumerable<RolesToReturn>> GetRolesAsync()
        {
            var roles = await roleManager.Roles.ToListAsync();
            var result = roles.Select(role => new RolesToReturn
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty
            }).ToList();

            return result;
        }




        public async Task DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role is null)
            {
                throw new NotFoundExeption(nameof(role), id);
            }
            await roleManager.DeleteAsync(role!);
        }
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

        public async Task<IEnumerable<AttendencesViewModel>> GetAllAttendences()
        {

            var users = await userManager.Users.Where(u => u.Types == Types.Attendee)
    .Select(u => new AttendencesViewModel
    {
        Id = u.Id,
        FullName = u.FullName!,
        PhoneNumber = u.PhoneNumber!,
        Email = u.Email!,
        Types = u.Types.ToString()
    })
    .ToListAsync();

            foreach (var user in users)
            {
                // Await the GetRolesAsync call properly here
                user.Roles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(user.Id));
            }

            return users;

        }

        public async Task<UserToReturn> CreateAttendences(CreateAttendenceDro createUserDro)
        {
            var user = new ApplicationUser
            {
                FullName = createUserDro.Name,
                PhoneNumber = createUserDro.PhoneNumber,
                Types = createUserDro.Type,
                Email = createUserDro.Email
            };


            var email = await userManager.FindByEmailAsync(user.Email);
            if (email is not null) throw new BadRequestExeption($" Email is Already Exsist ,Please Enter Anthor Email!");


            var result = await userManager.CreateAsync(user, createUserDro.Password);

            if (!result.Succeeded)
                throw new ValidationExeption() { Errors = result.Errors.Select(E => E.Description) };



            // Assign the "User" role to the newly created user
            var roleResult = await userManager.AddToRoleAsync(user, Types.Attendee.ToString());
            if (!roleResult.Succeeded)
                throw new ValidationExeption() { Errors = roleResult.Errors.Select(E => E.Description) };

            //var refresktoken = GenerateRefreshToken();

            //user.RefreshTokens.Add(new RefreshToken()
            //{
            //    Token = refresktoken.Token,
            //    ExpireOn = refresktoken.ExpireOn
            //});

            await userManager.UpdateAsync(user);

            var response = new UserToReturn
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email!,
                Types = user.Types.ToString(),
                FullName = user.FullName,
                Token = await GenerateTokenAsync(user),
                RefreshToken = "",
                RefreshTokenExpirationDate = DateTime.Now

            };

            return response;
        }

        public async Task<AttendentRoleViewModel> GetAttendence(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null) throw new NotFoundExeption("User Not Found", nameof(id));
            var allRoles = await roleManager.Roles.ToListAsync();
            var viewModel = new AttendentRoleViewModel()
            {
                Id = user.Id,
                Name = user.FullName!,
                PhoneNumber = user.PhoneNumber!,
                Email = user.Email!,
                Types = user.Types.ToString(),
                Roles = allRoles.Select(
                    r => new RoleDto()
                    {
                        Id = r.Id,
                        Name = r.Name!,
                        IsSelected = userManager.IsInRoleAsync(user, r.Name).Result
                    }).Where(u => u.IsSelected == true).ToList()
            };

            return viewModel;
        }

        public async Task<string> DeleteAttendence(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null) throw new NotFoundExeption("User Not Found", nameof(id));

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
                return "Delete Successed";
            else
            {
                return "Operation Faild";
            }


        }
        public async Task<IEnumerable<OrganizerViewModel>> GetAllOrganizers()
        {
            var techs = await userManager.Users.Where(u => u.Types == Types.Organizer)
   .Select(u => new OrganizerViewModel
   {
       Id = u.Id,
       FullName = u.FullName!,
       PhoneNumber = u.PhoneNumber!,
       Email = u.Email!,
       Types = u.Types.ToString(),
   })
   .ToListAsync();

            foreach (var tech in techs)
            {
                // Await the GetRolesAsync call properly here
                tech.Roles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(tech.Id));
            }

            return techs;
        }


    }
}
