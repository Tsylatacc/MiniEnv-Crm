namespace MiniEnv.Infrastructure.DTOs.Common.Authentication
{
    public sealed record JwtDto
    {
        public required string Token { get; init; }
        public required DateTimeOffset ExpiresAt { get; init; }
    }
}
