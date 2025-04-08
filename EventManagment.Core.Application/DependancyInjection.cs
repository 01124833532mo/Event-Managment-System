using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Services.Auth;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Application.Abstraction.Services.Emails;
using EventManagment.Core.Application.Abstraction.Services.Events;
using EventManagment.Core.Application.Abstraction.Services.FeedBacks;
using EventManagment.Core.Application.Abstraction.Services.Registrations;
using EventManagment.Core.Application.Abstraction.Services.Sesstions;
using EventManagment.Core.Application.Abstraction.Services.Speakers;
using EventManagment.Core.Application.Abstraction.Services.Sponsers;
using EventManagment.Core.Application.Mapping;
using EventManagment.Core.Application.Services.Auth;
using EventManagment.Core.Application.Services.Categories;
using EventManagment.Core.Application.Services.Emails;
using EventManagment.Core.Application.Services.Events;
using EventManagment.Core.Application.Services.FeedBacks;
using EventManagment.Core.Application.Services.Registrations;
using EventManagment.Core.Application.Services.Sesstions;
using EventManagment.Core.Application.Services.Speakers;
using EventManagment.Core.Application.Services.Sponsers;
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
            services.AddScoped(typeof(IRegistrationService), typeof(RegistrationService));
            services.AddScoped(typeof(IFeedBackService), typeof(FeedBackService));
            services.AddScoped(typeof(ISponserService), typeof(SponserService));
            services.AddScoped(typeof(ISpeakerService), typeof(SpeakersService));
            services.AddScoped(typeof(ISesstionService), typeof(SesstionService));
            services.AddTransient(typeof(IEmailService), typeof(EmailService));


            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IAuthService>();

            });

            services.AddScoped(typeof(Func<IRegistrationService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IRegistrationService>();

            });

            services.AddScoped(typeof(Func<IEventServices>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IEventServices>();

            });

            services.AddScoped(typeof(Func<ICategoryService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<ICategoryService>();

            });

            services.AddScoped(typeof(Func<IFeedBackService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IFeedBackService>();

            });

            services.AddScoped(typeof(Func<ISponserService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<ISponserService>();

            });

            services.AddScoped(typeof(Func<ISpeakerService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<ISpeakerService>();

            });

            services.AddScoped(typeof(Func<ISesstionService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<ISesstionService>();

            });
            return services;
        }

    }
}
