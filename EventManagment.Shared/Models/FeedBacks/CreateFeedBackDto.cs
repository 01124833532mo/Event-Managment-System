namespace EventManagment.Shared.Models.FeedBacks
{
    public class CreateFeedBackDto
    {
        //decimal rate, string comment, int EventId

        public decimal Rate { get; set; }
        public string Comment { get; set; }
        public int EventId { get; set; }
    }
}
