namespace EventManagment.Shared.Models.Registrations
{
    public class RegisterToReturn
    {
        public string PaymentStatus { get; set; }
        public decimal ServicePrice { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public string AttendeeId { get; set; }
        public required string FullName { get; set; }


        public int Eventid { get; set; }
        public required string EventTitle { get; set; }
        public required string CategoryName { get; set; }

        public required string EventDate { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; } = null!;


        public DateTime LastModifiedOn { get; set; }

    }
}
