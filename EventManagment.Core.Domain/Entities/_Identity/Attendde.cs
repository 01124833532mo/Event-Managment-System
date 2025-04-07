using EventManagment.Core.Domain.Entities.FeedBacks;
using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Core.Domain.Entities.Waitlists;

namespace EventManagment.Core.Domain.Entities._Identity
{
    public class Attendde : ApplicationUser
    {
        public DateOnly? BirthDate { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new HashSet<Feedback>();
        public virtual ICollection<WaitList>? WaitLists { get; set; } = new HashSet<WaitList>();

    }
}
