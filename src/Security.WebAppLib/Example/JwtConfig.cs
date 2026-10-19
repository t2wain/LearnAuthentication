namespace Security.WebAppLib.Example
{
    public class JwtConfig
    {
        public string Secret { get; set; } = null!;
        public string AuthenticationType { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Issuer { get; set; } = null!;

    }
}
