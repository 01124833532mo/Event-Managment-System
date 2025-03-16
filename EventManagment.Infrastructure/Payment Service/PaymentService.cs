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

        public async Task<RegisterToReturn> CreateOrUpdatePaymentIntent(int registerid, CancellationToken cancellationToken = default)
        {
            StripeConfiguration.ApiKey = _stripSettings.SecretKey;
            var repo = unitOfWork.GetRepository<Registration, int>();

            var register = await repo.GetAsync(registerid, cancellationToken);
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
                case "charge.refunded":
                    var chargeRefunded = (Charge)stripeEvent.Data.Object;
                    await UpdateRefundStatus(chargeRefunded.PaymentIntentId, isRefunded: true);
                    logger.LogInformation("Refund processed for PaymentIntentId: {0}", chargeRefunded.PaymentIntentId);
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

        private async Task<Registration> UpdateRefundStatus(string paymentIntentId, bool isRefunded)
        {
            var repo = unitOfWork.GetRepository<Registration, int>();
            var spec = new RegistrationByPaymentIntentSpecifications(paymentIntentId);
            var registration = await repo.GetWithSpecAsync(spec, default);

            if (registration is null)
            {
                throw new NotFoundExeption(nameof(registration), $"PaymentIntentId: {paymentIntentId}");
            }

            registration.PaymentStatus = isRefunded ? PaymentStatus.PaymentCanceled : PaymentStatus.PaymentFailed;
            repo.Update(registration);

            await unitOfWork.CompleteAsync();
            return registration;
        }
        public async Task<RegisterToReturn> CancelRegistrationAndRefund(int registerId, CancellationToken cancellation = default)
        {
            StripeConfiguration.ApiKey = _stripSettings.SecretKey;

            var repo = unitOfWork.GetRepository<Registration, int>();
            var register = await repo.GetAsync(registerId, cancellation);

            if (register is null)
            {
                throw new NotFoundExeption("Registration not found.", nameof(registerId));
            }

            if (string.IsNullOrEmpty(register.PaymentIntentId))
            {
                throw new BadRequestExeption("No payment intent associated with this registration.");
            }

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.GetAsync(register.PaymentIntentId, cancellationToken: cancellation);

            if (paymentIntent.Status != "succeeded")
            {
                throw new BadRequestExeption("The payment has not been successfully completed. Refund cannot be processed.");
            }

            var refundService = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = register.PaymentIntentId,
                Amount = (long)register.ServicePrice * 100,
                Reason = RefundReasons.RequestedByCustomer
            };

            try
            {
                var refund = await refundService.CreateAsync(refundOptions);

                register.PaymentStatus = PaymentStatus.PaymentCanceled;
                repo.Update(register);

                var complete = await unitOfWork.CompleteAsync() > 0;
                if (!complete)
                {
                    throw new BadRequestExeption("Failed to update registration status.");
                }

                logger.LogInformation("Refund processed successfully for registration ID: {0}", registerId);

                var result = mapper.Map<RegisterToReturn>(register);
                return result;
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Failed to process refund for registration ID: {0}", registerId);
                throw new BadRequestExeption("Failed to process refund. Please try again later.");
            }
        }
    }
}
