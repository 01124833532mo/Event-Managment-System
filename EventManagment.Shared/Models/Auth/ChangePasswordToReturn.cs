namespace EventManagment.Shared.Models.Auth
{
    public class ChangePasswordToReturn
    {
        public required string Message { get; set; }
        public required string Token { get; set; }
    }
}
