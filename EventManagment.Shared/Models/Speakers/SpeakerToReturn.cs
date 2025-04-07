namespace EventManagment.Shared.Models.Speakers
{
    public class SpeakerToReturn
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Bio { get; set; }
        public required string PhotoUrl { get; set; }

    }
}
