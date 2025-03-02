using EventManagment.Core.Domain._Identity;
using EventManagment.Infrastructure.Persistence._Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace EventManagment.Apis.Extintions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>
            {
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = true;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(5);
                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.SignIn.RequireConfirmedEmail = false;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = false;

            })
              .AddEntityFrameworkStores<EventManagmentDbContext>();

            services.AddAuthentication((configurationOptions =>
            {
                configurationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configurationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }))
                .AddJwtBearer(configurations =>
                {
                    configurations.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromHours(0),
                        ValidAudience = configuration["JwtSettings:Audience"],
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                    };
                });

            return services;

        }

    }
}
