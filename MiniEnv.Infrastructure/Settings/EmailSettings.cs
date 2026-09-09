namespace MiniEnv.Infrastructure.Settings
{
    public sealed class EmailSettings
    {
        public required string Host { get; init; } = default!;
        public required int Port { get; init; }
        public required string User { get; init; } = default!;
        public required string Password { get; init; } = default!;
        public required string FromName { get; init; } = default!;
    }
}