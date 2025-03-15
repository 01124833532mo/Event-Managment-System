using EventManagment.Shared.Models.Registrations;

namespace EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture
{
    public interface IPaymentService
    {
        public Task<RegisterToReturn> CreateOrUpdatePaymentIntent(int registerid);
        Task UpdateOrderPaymentStatus(string requestBody, string header);
    }
}
