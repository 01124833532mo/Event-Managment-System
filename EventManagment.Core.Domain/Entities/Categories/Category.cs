using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities.Events;

namespace EventManagment.Core.Domain.Entities.Categories
{
    public class Category : BaseAuditableEntity<int>
    {
        public required string Name { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();

    }
}
