namespace TaskSubscription.Application.Utils
{
    public sealed class AppSettings
    {
        public JwtSettings jwtSettings { get; set; }
    }

   


    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public byte ExpirationDaysInMinutes { get; set; }
        public byte ExpirationRefreshTokenInMinutes { get; set; }
        public string Encryptkey { get; set; }
    }
}
