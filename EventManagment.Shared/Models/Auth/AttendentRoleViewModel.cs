namespace EventManagment.Shared.Models.Auth
{
    public class AttendentRoleViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }

        public required string Types { get; set; }

        public IEnumerable<object> Roles { get; set; }
    }
}
