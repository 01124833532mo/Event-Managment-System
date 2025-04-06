using EventManagment.Core.Domain.Entities.FeedBacks;

namespace EventManagment.Core.Domain.Specifications.FeedBacks
{
    public class FeedBackWithFilterationForCountSpecifications : BaseSpecification<Feedback, int>
    {
        public FeedBackWithFilterationForCountSpecifications(int? eventid, string? attendeeid)
            : base(
                p =>
                (!eventid.HasValue || p.EventId == eventid.Value)
                &&
                (string.IsNullOrEmpty(attendeeid) || p.AttenddeId == attendeeid)
            )
        {
        }

        public FeedBackWithFilterationForCountSpecifications(int id) : base(id)
        {
        }
    }
}
