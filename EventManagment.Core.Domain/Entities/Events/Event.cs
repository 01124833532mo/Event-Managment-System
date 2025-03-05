using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Common;
using EventManagment.Core.Domain.Entities.Notifications;
using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Core.Domain.Enums;

namespace EventManagment.Core.Domain.Entities.Events
{
    public class Event : BaseAuditableEntity<int>, IBaseOrganzer
    {
        //        + Id: int
        //+ Title: string
        //+ Description: string
        //+ DateTime: DateTime
        //+ Location: string
        //+ MaxAttendees: int
        //+ OrganizerId: int
        //+ Status: string
        //+ PaymentRequired: bool
        //+ Organizer: User
        //+ Registrations: List<Registration>
        //+ PaymentTransactions: List<PaymentTransaction>
        //+ Notifications: List<Notification>
        //+ CategoryId: int
        //+ Category: Category

        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime Data { get; set; }
        public required string Location { get; set; }
        public required int MaxAttendees { get; set; }
        public EventStatus Status { get; set; }

        public string? OrganizerId { get; set; }
        public virtual ApplicationUser Organizer { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; } = new HashSet<Registration>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();

        public int? Categoryid { get; set; }

        public virtual Category Category { get; set; }


    }
}
