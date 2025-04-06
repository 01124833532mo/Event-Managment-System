using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        IEmailService emailService, ILogger<AuthService> logger,
        IAttachmentService attachmentService,
        IConfiguration configuration) : IAuthService
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
        public async Task<BaseToReturn> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                logger.LogWarning("Invalid Login Attempt For Email {Email}", loginDto.Email);
                throw new UnAuthorizedExeption("Invalid Login");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            if (result.IsNotAllowed)
                throw new UnAuthorizedExeption("Email is Not Confirmed");

            if (result.IsLockedOut)
                throw new UnAuthorizedExeption("Email is Locked Out");

            if (!result.Succeeded)
                throw new UnAuthorizedExeption("Invalid Login");

            var userroles = await userManager.GetRolesAsync(user);


            if (userroles.Any(role => role == Roles.Organizer))
            {
                var response = new OrganizerToReturn
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber!,
                    Email = user.Email!,
                    Age = ((Organizer)user).Age,
                    Address = ((Organizer)user).Address!,
                    CompanyName = ((Organizer)user).CompanyName!,
                    Types = user.Types.ToString(),
                    Token = await GenerateTokenAsync(user),
                    PictureUrl = $"{configuration["Urls:ApiBaseUrl"]}/{user.PictureUrl}",
                };
                await CheckRefreshToken(userManager, user, response);

                return response;
            }
            else
            {
                var response = new AttendeeToReturn
                {
                    Id = user.Id,
                    FullName = user.FullName!,
                    PhoneNumber = user.PhoneNumber!,
                    Email = user.Email!,
                    BirthDate = ((Attendde)user).BirthDate,
                    Types = user.Types.ToString(),
                    Token = await GenerateTokenAsync(user),
                    PictureUrl = $"{configuration["Urls:ApiBaseUrl"]}/{user.PictureUrl}",


                };
                await CheckRefreshToken(userManager, user, response);

                return response;
            }
        }
        public async Task<BaseToReturn> RegisterAsync(RegisterDto registerDto)
        {
            if (userManager.Users.Any(e => e.Email == registerDto.Email))
                throw new BadRequestExeption("Email Already Exists");

            var user = registerDto.Types == Types.Organizer
                ? (ApplicationUser)CreateOrganizer(registerDto)
                : CreateAttendee(registerDto);

            if (registerDto.PictureUrl is not null)
            {
                var uploadedImageUrl = await attachmentService.UploadAsynce(registerDto.PictureUrl, "ProfilePicture");
                user.PictureUrl = uploadedImageUrl;
            }

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new ValidationExeption { Errors = result.Errors.Select(p => p.Description) };

            await ConfirmationCodeSendByEmailAsync(new ForgetPasswordByEmailDto { Email = user.Email! });

            string role = registerDto.Types == Types.Organizer ? Roles.Organizer.ToString() : Roles.Attendee.ToString();
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
                throw new ValidationExeption { Errors = roleResult.Errors.Select(e => e.Description) };

            return CreateUserResponse(user);
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


        public async Task<BaseToReturn> CreateAttendences(CreateAttendenceDro createUserDro)
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

            var response = new BaseToReturn
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
                Subject = "Reset Code For Event Managment Account",
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

        public async Task<BaseToReturn> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetCodeDto)
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

            var mappedUser = new BaseToReturn
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

        public async Task<BaseToReturn> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default)
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

            return new BaseToReturn()
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
        private async Task CheckRefreshToken(UserManager<ApplicationUser> userManager, ApplicationUser? user, BaseToReturn response)
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

        public async Task<BaseToReturn> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email!);
            if (user is null) throw new NotFoundExeption("User Not Found", nameof(email));
            var role = await userManager.GetRolesAsync(user);

            if (role.Any(r => r == Roles.Organizer))
            {
                var org = (Organizer)user;
                return new OrganizerToReturn()
                {
                    Id = org.Id,
                    Email = org.Email!,
                    FullName = org.FullName,
                    PhoneNumber = org.PhoneNumber!,
                    Types = user.Types.ToString(),
                    PictureUrl = $"{configuration["Urls:ApiBaseUrl"]}/{user.PictureUrl}",
                    Token = await GenerateTokenAsync(user),
                    Age = org.Age,
                    Address = org.Address!,
                    CompanyName = org.CompanyName!
                };
            }
            else if (role.Any(r => r == Roles.Attendee))
            {
                var att = (Attendde)user;
                return new AttendeeToReturn()
                {
                    Id = att.Id,
                    Email = att.Email!,
                    FullName = att.FullName,
                    PhoneNumber = att.PhoneNumber!,
                    Types = user.Types.ToString(),
                    PictureUrl = $"{configuration["Urls:ApiBaseUrl"]}/{user.PictureUrl}",
                    Token = await GenerateTokenAsync(user),
                    BirthDate = att.BirthDate
                };
            }
            else if (role.Any(r => r == Roles.Admin))
            {
                return new BaseToReturn
                {

                    Id = user.Id,
                    Email = user.Email!,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber!,
                    Types = user.Types.ToString(),
                    PictureUrl = $"{configuration["Urls:ApiBaseUrl"]}/{user.PictureUrl}",
                    Token = await GenerateTokenAsync(user),
                };
            }
            throw new NotFoundExeption("User Not Found", nameof(email));


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

        public async Task<SuccessDto> ConfirmationCodeSendByEmailAsync(ForgetPasswordByEmailDto emailDto)
        {
            var result = await ForgetPasswordByEmailasync(emailDto);

            return result;
        }
        public async Task<SuccessDto> ConfirmEmailAsync(ConfirmationEmailCodeDto codeDto)
        {
            var user = await userManager.Users.Where(U => U.Email == codeDto.Email).FirstOrDefaultAsync();

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            if (user.ResetCode != codeDto.ConfirmationCode)
                throw new BadRequestExeption("The Provided Code Is Invalid");

            if (user.ResetCodeExpiry < DateTime.UtcNow)
                throw new BadRequestExeption("The Provided Code Has Been Expired");

            user.EmailConfirmed = true;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Confirming Email");

            var SuccessObj = new SuccessDto()
            {
                Status = "Success",
                Message = "Email Has Been Confirmed"
            };

            return SuccessObj;
        }

        private Organizer CreateOrganizer(RegisterDto dto)
        {
            return new Organizer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Types = dto.Types,
                Age = dto.Age,
                Address = dto.Address,
                CompanyName = dto.CompanyName
            };
        }

        private Attendde CreateAttendee(RegisterDto dto)
        {
            return new Attendde
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Types = dto.Types,
                BirthDate = dto.BirthDate
            };
        }

        private BaseToReturn CreateUserResponse(ApplicationUser user)
        {


            if (user is Organizer org)
            {
                var response = new OrganizerToReturn
                {

                    Id = org.Id,
                    FullName = org.FullName,
                    Email = org.Email!,
                    PhoneNumber = org.PhoneNumber!,
                    Types = org.Types.ToString(),
                    Token = GenerateTokenAsync(org).Result,
                    PictureUrl = org.PictureUrl != null ? $"{configuration["Urls:ApiBaseUrl"]}/{user.PictureUrl}" : null,
                    Age = org.Age,
                    Address = org.Address!,
                    CompanyName = org.CompanyName!,
                };

                return response;
            }
            else if (user is Attendde att)
            {
                var response = new AttendeeToReturn
                {

                    Id = att.Id,
                    FullName = att.FullName,
                    Email = att.Email!,
                    PhoneNumber = att.PhoneNumber!,
                    Types = att.Types.ToString(),
                    Token = GenerateTokenAsync(att).Result,
                    PictureUrl = att.PictureUrl != null ? $"{configuration["Urls:ApiBaseUrl"]}/{user.PictureUrl}" : null,
                    BirthDate = att.BirthDate!.Value,
                };

                return response;
            }
            return null!;

        }
    }

}


