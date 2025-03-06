using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Contracts.Persestence.DbInitializers;
using EventManagment.Infrastructure.Persistence._Data;
using EventManagment.Infrastructure.Persistence._Data.Interceptors;
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
            services.AddDbContext<EventManagmentDbContext>((provider, options) =>
            {
                options.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("EventManagmentContext"))
                .AddInterceptors(provider.GetRequiredService<AuditInterceptor>()
                , provider.GetRequiredService<SettedUserIdInterceptor>(),
                provider.GetRequiredService<SettedOrganizerIdInterseptor>())
                ;

            });
            services.AddScoped(typeof(AuditInterceptor));
            services.AddScoped(typeof(SettedUserIdInterceptor));
            services.AddScoped(typeof(SettedOrganizerIdInterseptor));
            services.AddScoped(typeof(IEventManagmentDbInitializer), typeof(EventMangmentDbInitilzer));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));
            return services;
        }
    }
}
