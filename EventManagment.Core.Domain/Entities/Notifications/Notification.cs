using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Common;
using EventManagment.Core.Domain.Entities.Events;

namespace EventManagment.Core.Domain.Entities.Notifications
{
    public class Notification : BaseAuditableEntity<int>, IBaseUserId
    {
        public required string Message { get; set; }
        public DateTime Date { get; set; }

        public string? AttendeeId { get; set; }

        public virtual ApplicationUser Attendee { get; set; }

        public int? eventid { get; set; }
        public virtual Event Event { get; set; }

    }
}
