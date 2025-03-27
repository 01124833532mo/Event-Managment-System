namespace EventManagment.Shared.Models.Auth
{
    public class OrganizerToReturn : BaseToReturn
    {
        public int? Age { get; set; }
        public required string Address { get; set; }
        public required string CompanyName { get; set; }

    }
}
