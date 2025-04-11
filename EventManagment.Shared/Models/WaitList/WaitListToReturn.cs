namespace EventManagment.Shared.Models.WaitList
{
    public class WaitListToReturn
    {
        public DateTime? JoinDate { get; set; }
        public string Status { get; set; } = null!;

        public bool IsNotified { get; set; }
        public string FullName { get; set; } = null!;

        public string EventTitle { get; set; } = null!;


    }
}
