using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Common;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Enums;

namespace EventManagment.Core.Domain.Entities.Registrations
{
    //order
    public class Registration : BaseAuditableEntity<int>, IBaseUserId
    {
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public string AttendeeId { get; set; }
        public virtual ApplicationUser Attendee { get; set; }


        public int Eventid { get; set; }
        public virtual Event Event { get; set; }
    }
}
