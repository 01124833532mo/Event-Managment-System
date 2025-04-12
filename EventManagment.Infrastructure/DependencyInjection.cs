using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Infrastructure.AttachementService;
using EventManagment.Infrastructure.Caching_Service;
using EventManagment.Infrastructure.Payment_Service;
using EventManagment.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace EventManagment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IAttachmentService), typeof(AttachmentService));
            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));


            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceprovider) =>
            {
                var connectionstring = configuration.GetConnectionString("Redis");
                var multiplexer = ConnectionMultiplexer.Connect(connectionstring!);
                return multiplexer;

            });

            services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));


            return services;
        }
    }
}
