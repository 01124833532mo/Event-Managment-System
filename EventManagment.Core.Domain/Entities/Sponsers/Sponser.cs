using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities.Events;

namespace EventManagment.Core.Domain.Entities.Sponsers
{
    public class Sponser : BaseEntity<int>
    {

        public required string Name { get; set; }
        public required string LogoUrl { get; set; }
        public required string Website { get; set; }
        public required string Description { get; set; }

        // navigation Property
        public virtual ICollection<Event>? Events { get; set; } = new HashSet<Event>();
    }
}
