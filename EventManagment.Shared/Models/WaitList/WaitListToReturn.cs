namespace EventManagment.Shared.Models.WaitList
{
    public class WaitListToReturn
    {
        public int Id { get; set; }
        public DateTime? JoinDate { get; set; }
        public string Status { get; set; } = null!;

        public bool IsNotified { get; set; }
        public string FullName { get; set; } = null!;

        public string EventTitle { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; } = null!;


        public DateTime LastModifiedOn { get; set; }


    }
}
