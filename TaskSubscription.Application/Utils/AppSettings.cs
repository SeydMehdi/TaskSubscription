namespace TaskSubscription.Application.Utils
{
    public sealed class AppSettings
    {
        public JwtSettings jwtSettings { get; set; }
        public IdentitySettings identitySettings { get; set; }
       public string BaseFileUrl { get; set; }
    }

    public class IdentitySettings
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumeric { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }


    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public byte ExpirationDays { get; set; }
        public string Encryptkey { get; set; }
    }
}
