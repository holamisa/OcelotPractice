namespace Middleware.Jwt
{
    public class JwtOptions
    {
        public required string Secret { get; set; }
        public required int ExpirationInMinutes { get; set; }
    }
}
