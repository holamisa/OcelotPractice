namespace Middleware.Jwt
{
    public class JwtOptionsDto
    {
        public required string Secret { get; set; }
        public required int ExpirationInMinutes { get; set; }
    }
}
