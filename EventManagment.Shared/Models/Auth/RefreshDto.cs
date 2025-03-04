namespace EventManagment.Shared.Models.Auth
{
    public class RefreshDto
    {
        public required string Token { get; set; }

        public required string RefreshToken { get; set; }
    }
}
