using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Shared.Models.Enums;

namespace EventManagment.Core.Domain.Entities.Waitlists
{
    public class WaitList : BaseAuditableEntity<int>
    {

        public DateTime? JoinDate { get; set; }
        public WaitListStatus Status { get; set; } = WaitListStatus.Pending;

        public bool IsNotified { get; set; } = false;


        // Navigation properties
        public string? AttendeeId { get; set; } = null!;

        public virtual Attendde? Attendde { get; set; } = null!;
        public int EventId { get; set; }

        public virtual Event Event { get; set; } = null!;
    }
}
