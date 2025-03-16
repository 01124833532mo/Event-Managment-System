using EventManagment.Shared.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventManagment.Shared
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddSharedDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<StripSettings>(configuration.GetSection("StripeSettings"));

            services.AddHangfire(h => h.UseSqlServerStorage(configuration.GetConnectionString("EventManagmentContext")))
                    .AddHangfireServer();

            return services;
        }
    }
}
