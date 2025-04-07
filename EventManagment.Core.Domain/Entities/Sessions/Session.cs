using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.Speakers;

namespace EventManagment.Core.Domain.Entities.Sessions
{
    public class Session : BaseAuditableEntity<int>
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public int? SpeakerId { get; set; }
        public virtual Speaker? Speaker { get; set; }


    }
}
