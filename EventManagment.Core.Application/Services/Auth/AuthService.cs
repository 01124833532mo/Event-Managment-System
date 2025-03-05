using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Emails;
using EventManagment.Core.Domain._Identity;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models._Common.Emails;
using EventManagment.Shared.Models.Auth;
using EventManagment.Shared.Models.Roles;
using EventManagment.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventManagment.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager
        , SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager
        , IOptions<JwtSettings> jwtsettings,
        IEmailService emailService) : IAuthService
    {
        private readonly JwtSettings _jwtsettings = jwtsettings.Value;

        #region Roles
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
        #endregion

        #region Login & Register
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

                };
                await CheckRefreshToken(userManager, user, response);

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

                };
                await CheckRefreshToken(userManager, user, response);

                return response;
            }
        }
        public async Task<UserToReturn> RegisterAsync(RegisterDto registerDto)
        {

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

            var refresktoken = GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken()
            {
                Token = refresktoken.Token,
                ExpireOn = refresktoken.ExpireOn
            });

            await userManager.UpdateAsync(user);



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

                RefreshToken = refresktoken.Token,
                RefreshTokenExpirationDate = refresktoken.ExpireOn,
            };

            return response;
        }
        #endregion




        #region Crud => Attendee and Oeganaizer

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



        #endregion
        public async Task<SuccessDto> ForgetPasswordByEmailasync(ForgetPasswordByEmailDto emailDto)
        {
            var user = await userManager.Users.Where(u => u.Email == emailDto.Email).FirstOrDefaultAsync();

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            var ResetCode = RandomNumberGenerator.GetInt32(100_000, 999_999);

            var ResetCodeExpire = DateTime.UtcNow.AddMinutes(15);

            user.ResetCode = ResetCode;
            user.ResetCodeExpiry = ResetCodeExpire;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Sending Reset Code");

            var Email = new Email()
            {
                To = emailDto.Email,
                Subject = "Reset Code For CarCare Account",
                Body = $"We Have Recived Your Request For Reset Your Account Password, \nYour Reset Code Is ==> [ {ResetCode} ] <== \nNote: This Code Will Be Expired After 15 Minutes!",
            };

            await emailService.SendEmail(Email);

            var SuccessObj = new SuccessDto()
            {
                Status = "Success",
                Message = "We Have Sent You The Reset Code"
            };

            return SuccessObj;
        }

        public async Task<SuccessDto> VerifyCodeByEmailAsync(ResetCodeConfirmationByEmailDto resetCodeDto)
        {
            var user = await userManager.Users.Where(u => u.Email == resetCodeDto.Email).FirstOrDefaultAsync();

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            if (user.ResetCode != resetCodeDto.ResetCode)
                throw new BadRequestExeption("The Provided Code Is Invalid");

            if (user.ResetCodeExpiry < DateTime.UtcNow)
                throw new BadRequestExeption("The Provided Code Has Been Expired");

            var SuccessObj = new SuccessDto()
            {
                Status = "Success",
                Message = "Reset Code Is Verified, Please Proceed To Change Your Password"
            };

            return SuccessObj;
        }

        public async Task<UserToReturn> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetCodeDto)
        {
            var user = await userManager.Users.Where(u => u.Email == resetCodeDto.Email).FirstOrDefaultAsync();

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            var RemovePass = await userManager.RemovePasswordAsync(user);

            if (!RemovePass.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

            var newPass = await userManager.AddPasswordAsync(user, resetCodeDto.NewPassword);

            if (!newPass.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

            var mappedUser = new UserToReturn
            {
                FullName = user.FullName!,
                Id = user.Id,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user),
                PhoneNumber = user.PhoneNumber,
                Types = user.Types.ToString(),

            };

            if (user!.RefreshTokens.Any(t => t.IsActice))
            {
                var acticetoken = user.RefreshTokens.FirstOrDefault(x => x.IsActice);
                mappedUser.RefreshToken = acticetoken!.Token;
                mappedUser.RefreshTokenExpirationDate = acticetoken.ExpireOn;
            }
            else
            {

                var refreshtoken = GenerateRefreshToken();
                mappedUser.RefreshToken = refreshtoken.Token;
                mappedUser.RefreshTokenExpirationDate = refreshtoken.ExpireOn;

                user.RefreshTokens.Add(new RefreshToken()
                {
                    Token = refreshtoken.Token,
                    ExpireOn = refreshtoken.ExpireOn,
                });
                await userManager.UpdateAsync(user);
            }

            return mappedUser;
        }
        #region Refresh Token

        public async Task<UserToReturn> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default)
        {
            var userId = ValidateToken(refreshDto.Token);

            if (userId is null) throw new NotFoundExeption("User id Not Found", nameof(userId));

            var user = await userManager.FindByIdAsync(userId);
            if (user is null) throw new NotFoundExeption("User Do Not Exists", nameof(user.Id));

            var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

            if (UserRefreshToken is null) throw new NotFoundExeption("Invalid Token", nameof(userId));

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            var newtoken = await GenerateTokenAsync(user);

            var newrefreshtoken = GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken()
            {
                Token = newrefreshtoken.Token,
                ExpireOn = newrefreshtoken.ExpireOn
            });

            await userManager.UpdateAsync(user);

            return new UserToReturn()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Types = user.Types.ToString(),

                Token = newtoken,
                RefreshToken = newrefreshtoken.Token,
                RefreshTokenExpirationDate = newrefreshtoken.ExpireOn,


            };
        }

        public async Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default)
        {
            var userId = ValidateToken(refreshDto.Token);

            if (userId is null) return false;

            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return false;

            var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

            if (UserRefreshToken is null) return false;

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(user);
            return true;
        }
        #endregion




        #region Custome Functions
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
        private string? ValidateToken(string token)
        {
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.Key));

            var tokenhandler = new JwtSecurityTokenHandler();

            try
            {
                tokenhandler.ValidateToken(token, new TokenValidationParameters()
                {
                    IssuerSigningKey = authkey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken securityToken);

                var securitytokenobj = (JwtSecurityToken)securityToken;

                return securitytokenobj.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;
            }
            catch (Exception)
            {

                return null;
            }
        }
        private RefreshToken GenerateRefreshToken()
        {

            var randomNumber = new byte[32];

            var genrator = new RNGCryptoServiceProvider();

            genrator.GetBytes(randomNumber);

            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddDays(_jwtsettings.JWTRefreshTokenExpire)


            };


        }
        private async Task CheckRefreshToken(UserManager<ApplicationUser> userManager, ApplicationUser? user, UserToReturn response)
        {
            if (user!.RefreshTokens.Any(t => t.IsActice))
            {
                var acticetoken = user.RefreshTokens.FirstOrDefault(x => x.IsActice);
                response.RefreshToken = acticetoken!.Token;
                response.RefreshTokenExpirationDate = acticetoken.ExpireOn;
            }
            else
            {

                var refreshtoken = GenerateRefreshToken();
                response.RefreshToken = refreshtoken.Token;
                response.RefreshTokenExpirationDate = refreshtoken.ExpireOn;

                user.RefreshTokens.Add(new RefreshToken()
                {
                    Token = refreshtoken.Token,
                    ExpireOn = refreshtoken.ExpireOn,
                });
                await userManager.UpdateAsync(user);
            }
        }
        #endregion

        public async Task<UserToReturn> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email!);

            return new UserToReturn()
            {
                Id = user!.Id,
                Email = user!.Email!,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Types = user.Types.ToString(),

                Token = await GenerateTokenAsync(user)

            };
        }

        public async Task<ChangePasswordToReturn> ChangePasswordAsync(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto)
        {
            var userId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value;

            if (userId is null) throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");


            // Retrieve the user from the database
            var user = await userManager.FindByIdAsync(userId);

            if (user is null) throw new NotFoundExeption("No User For This Id", nameof(userId));


            // Verify the current password
            var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);

            if (!isCurrentPasswordValid)
            {
                throw new BadRequestExeption("This Current Password InCorrect");
            }

            // Change the password
            var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                throw new ValidationExeption() { Errors = result.Errors.Select(p => p.Description) };
            }

            // Optionally, generate a new token for the user
            var newToken = await GenerateTokenAsync(user);

            return new ChangePasswordToReturn()
            {
                Message = "Password changed successfully.",
                Token = newToken
            };
        }
    }

}


