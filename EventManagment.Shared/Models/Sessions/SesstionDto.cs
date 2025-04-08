namespace EventManagment.Shared.Models.Sesstions
{
    public class SesstionDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public int EventId { get; set; }
        public int SpeakerId { get; set; }
    }
}
