namespace EventManagment.Shared.Settings
{
    public class JwtSettings
    {
        public required string Key { get; set; }
        public required string Audience { get; set; }
        public required string Issuer { get; set; }
        public required double DurationInMinitutes { get; set; }
        public required int JWTRefreshTokenExpire { get; set; }
    }
}
