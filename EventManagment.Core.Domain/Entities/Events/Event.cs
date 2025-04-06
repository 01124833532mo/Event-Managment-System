using EventManagment.Core.Domain.Common;
using EventManagment.Core.Domain.Entities._Identity;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Core.Domain.Entities.Common;
using EventManagment.Core.Domain.Entities.FeedBacks;
using EventManagment.Core.Domain.Entities.Notifications;
using EventManagment.Core.Domain.Entities.Registrations;
using EventManagment.Core.Domain.Entities.Sponsers;
using EventManagment.Core.Domain.Enums;

namespace EventManagment.Core.Domain.Entities.Events
{
    public class Event : BaseAuditableEntity<int>, IBaseOrganzer
    {


        public required string Title { get; set; }
        public required string NormalizedTitle { get; set; }
        public required string Description { get; set; }
        public DateTime Data { get; set; }
        public required string Location { get; set; }
        public required int MaxAttendees { get; set; }

        public decimal? EventRate { get; set; }

        public int EventCountRating { get; set; }



        public EventStatus Status { get; set; } = EventStatus.scheduled;

        public string? OrganizerId { get; set; }
        public virtual Organizer Organizer { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; } = new HashSet<Registration>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();

        public int? Categoryid { get; set; }

        public virtual Category Category { get; set; }



        public virtual ICollection<Feedback>? Feedbacks { get; set; } = new HashSet<Feedback>();


        public int? SponserId { get; set; }
        public virtual Sponser? Sponser { get; set; }


    }
}
