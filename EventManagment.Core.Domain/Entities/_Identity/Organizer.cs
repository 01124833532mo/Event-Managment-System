using EventManagment.Core.Domain.Entities.Events;

namespace EventManagment.Core.Domain.Entities._Identity
{
    public class Organizer : ApplicationUser
    {
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? CompanyName { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();

    }
}
