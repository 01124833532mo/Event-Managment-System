namespace EventManagment.Shared.Models.Events
{
    public class EventToreturn
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime Data { get; set; }
        public required string Location { get; set; }
        public required int MaxAttendees { get; set; }
        public string Status { get; set; }

        public string OrganizerId { get; set; }
        public string? OrganizerName { get; set; }

        public string? NameOfSponser { get; set; }

        public string? CategoryName { get; set; }

        public decimal EventRate { get; set; }

        public int EventCountRating { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; } = null!;


        public DateTime LastModifiedOn { get; set; }
    }
}
