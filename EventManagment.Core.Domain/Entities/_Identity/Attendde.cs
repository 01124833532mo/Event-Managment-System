using EventManagment.Core.Domain.Entities.FeedBacks;
using EventManagment.Core.Domain.Entities.Registrations;

namespace EventManagment.Core.Domain.Entities._Identity
{
    public class Attendde : ApplicationUser
    {
        public DateOnly? BirthDate { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new HashSet<Feedback>();

    }
}
