using EventManagment.Apis.Services;
using EventManagment.Core.Application.Abstraction;

namespace EventManagment.Apis.Extintions
{
    public static class DependencyInjection
    {

        public static IServiceCollection RegesteredPresestantLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));
            return services;
        }

    }
}
