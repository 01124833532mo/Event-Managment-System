namespace EventManagment.Shared.Models.Sesstions
{
    public class SesstionToreturn
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }

        public required string EventTitle { get; set; }
        public required string EventDescription { get; set; }


        //Speaker
        public required string SpeakerName { get; set; }
        public required string SpeakerBio { get; set; }
        public required string SpeakerPhotoUrl { get; set; }
    }
}
