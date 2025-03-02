namespace EventManagment.Shared.Models.Auth
{
    public class AttendencesViewModel
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }

        public required string Types { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
