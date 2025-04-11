using EventManagment.Core.Domain.Entities.Waitlists;

namespace EventManagment.Core.Domain.Specifications.WalitLists
{
    public class waitlistCountSpecification : BaseSpecification<WaitList, int>
    {
        public waitlistCountSpecification() : base(x => x.IsNotified == false)
        {
        }

    }
}
