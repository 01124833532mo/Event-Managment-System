using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities.Sessions;

namespace EventManagment.Core.Domain.Entities.Speakers
{
    public class Speaker : BaseEntity<int>
    {

        public required string Name { get; set; }
        public required string Bio { get; set; }
        public required string PhotoUrl { get; set; }


        // Foreign key to the Event entity

        public virtual ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
