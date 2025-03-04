﻿using Microsoft.AspNetCore.Identity;

namespace EventManagment.Core.Domain._Identity
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
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


    }
}
