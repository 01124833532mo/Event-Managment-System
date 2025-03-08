namespace EventManagment.Shared.Models.Events
{
    public class EventDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime Data { get; set; }
        public required string Location { get; set; }
        public required int MaxAttendees { get; set; }
        public int Categoryid { get; set; }
    }
}
