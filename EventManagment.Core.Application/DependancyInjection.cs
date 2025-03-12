using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Application.Abstraction.Services.Emails;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Application.Mapping;
using EventManagment.Core.Application.Services.Auth;
using EventManagment.Core.Application.Services.Categories;
using EventManagment.Core.Application.Services.Emails;
using EventManagment.Core.Application.Services.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagment.Core.Application
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IEventServices), typeof(EventService));
            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            services.AddTransient(typeof(IEmailService), typeof(EmailService));


            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IAuthService>();

            });

            services.AddScoped(typeof(Func<IEventServices>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IEventServices>();

            });

            return services;
        }

    }
}
