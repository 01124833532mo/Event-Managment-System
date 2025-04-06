using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Infrastructure.AttachementService;
using EventManagment.Infrastructure.Payment_Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IAttachmentService), typeof(AttachmentService));


            return services;
        }
    }
}
