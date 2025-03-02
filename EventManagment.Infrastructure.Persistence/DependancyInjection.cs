using EventManagment.Core.Domain.Contracts.Persestence.DbInitializers;
using EventManagment.Infrastructure.Persistence._Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagment.Infrastructure.Persistence
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventManagmentDbContext>((options) =>
            {
                options.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("EventManagmentContext"));

            });
            services.AddScoped(typeof(IEventManagmentDbInitializer), typeof(EventMangmentDbInitilzer));
            return services;
        }
    }
}
