using AutoMapper;
using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Core.Domain.Enums;
using EventManagment.Core.Domain.Specifications.Common;
using EventManagment.Shared.Errors.Models;
using EventManagment.Shared.Models.Registrations;
using EventManagment.Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;

namespace EventManagment.Infrastructure.Payment_Service
{
    public class PaymentService(IOptions<StripSettings> stripsettings,
       IUnitOfWork unitOfWork,
       IMapper mapper,
       ILogger<PaymentService> logger)
       : IPaymentService
    {
        private readonly StripSettings _stripSettings = stripsettings.Value;

        public async Task<RegisterToReturn> CreateOrUpdatePaymentIntent(int registerid)
        {
            StripeConfiguration.ApiKey = _stripSettings.SecretKey;
            var repo = unitOfWork.GetRepository<Registration, int>();

            var register = await repo.GetAsync(registerid);
            if (register is null) throw new NotFoundExeption("No register Exsist For ", nameof(registerid));

            PaymentIntent? paymentIntent = null;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(register.PaymentIntentId))
            {// create New PaymentIntent

                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)register.ServicePrice * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);     // integration with Stripe
                register.PaymentIntentId = paymentIntent.Id;
                register.ClientSecret = paymentIntent.ClientSecret;

            }
            else  // Update PaymentIntent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)register.ServicePrice * 100
                };

                await paymentIntentService.UpdateAsync(register.PaymentIntentId, options);      // integration with Stripe

            }
            repo.Update(register);
            var chickComplete = await unitOfWork.CompleteAsync() > 0;
            if (!chickComplete) throw new BadRequestExeption("There is an Error in Request");

            var result = mapper.Map<RegisterToReturn>(register);

            return result;
        }

        public async Task UpdateOrderPaymentStatus(string requestBody, string header)
        {
            var stripeEvent = EventUtility.ConstructEvent(requestBody, header, _stripSettings.WebhookSecret);

            // Handle the event

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            Registration? order;
            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    order = await UpdatePaymentIntent(paymentIntent.Id, isPaid: true);
                    logger.LogInformation("register is Succeeded With Payment IntentId:{0}", paymentIntent.Id);
                    break;
                case "payment_intent.payment_failed":
                    order = await UpdatePaymentIntent(paymentIntent.Id, isPaid: false);
                    logger.LogInformation("register is !Succeeded With Payment IntentId:{0}", paymentIntent.Id);


                    break;
            }
        }

        private async Task<Registration> UpdatePaymentIntent(string paymentIntentId, bool isPaid)
        {
            var orderRepo = unitOfWork.GetRepository<Registration, int>();

            var spec = new RegistrationByPaymentIntentSpecifications(paymentIntentId);
            var order = await orderRepo.GetWithSpecAsync(spec, default);

            if (order is null) throw new NotFoundExeption(nameof(order), $"PaymentIntentId :{paymentIntentId}");

            if (isPaid)
                order.PaymentStatus = PaymentStatus.PaymentReceived;
            else
                order.PaymentStatus = PaymentStatus.PaymentFailed;


            orderRepo.Update(order);

            await unitOfWork.CompleteAsync();
            return order;
        }
    }
}
