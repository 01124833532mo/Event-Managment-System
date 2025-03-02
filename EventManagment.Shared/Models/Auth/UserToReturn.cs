namespace EventManagment.Shared.Models.Auth
{
    public class UserToReturn
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Types { get; set; }
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpirationDate { get; set; }
    }

}
