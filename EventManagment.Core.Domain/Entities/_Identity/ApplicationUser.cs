using EventManagment.Core.Domain._Identity;
using EventManagment.Core.Domain.Entities.Events;
using EventManagment.Core.Domain.Entities.Notifications;
using EventManagment.Core.Domain.Entities.Registrations;
using Microsoft.AspNetCore.Identity;

namespace EventManagment.Core.Domain.Entities._Identity
{
    public class ApplicationUser : IdentityUser
    {
        //        + Id: int
        //+ Username: string
        //+ Email: string
        //+ PasswordHash: string
        //+ Role: string
        //+ Events: List<Event>
        //+ Registrations: List<Registration>
        //+ PaymentTransactions: List<PaymentTransaction>
        //+ Notifications: List<Notification>

        public required string FullName { get; set; }
        public Types Types { get; set; }
        public int? ResetCode { get; set; }
        public DateTime? ResetCodeExpiry { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();


    }
}
