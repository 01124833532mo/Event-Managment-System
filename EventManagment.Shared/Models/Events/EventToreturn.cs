namespace EventManagment.Shared.Models.Events
{
    public class EventToreturn
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime Data { get; set; }
        public required string Location { get; set; }
        public required int MaxAttendees { get; set; }
        public string Status { get; set; }

        public string OrganizerId { get; set; }
        public string? OrganizerName { get; set; }


        public string? CategoryName { get; set; }
    }
}
