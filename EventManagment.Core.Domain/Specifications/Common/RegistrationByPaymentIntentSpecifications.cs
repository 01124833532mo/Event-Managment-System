using EventManagment.Core.Domain.Entities.Registrations;

namespace EventManagment.Core.Domain.Specifications.Common
{
    public class RegistrationByPaymentIntentSpecifications : BaseSpecification<Registration, int>
    {
        public RegistrationByPaymentIntentSpecifications(string paymentintent) : base(
                    order => order.PaymentIntentId == paymentintent
            )
        {
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();

            Includes.Add(p => p.Event!);
            Includes.Add(p => p.Event!.Category!);
            Includes.Add(p => p.Attendee!);
        }

    }
}
