using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Contracts.Persestence.DbInitializers;
using EventManagment.Infrastructure.Persistence._Data;
using EventManagment.Infrastructure.Persistence.Repositories.Generic_Repository;
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
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));
            return services;
        }
    }
}
