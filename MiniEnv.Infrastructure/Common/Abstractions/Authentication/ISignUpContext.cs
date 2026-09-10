namespace MiniEnv.Infrastructure.Common.Abstractions.Authentication
{
    public interface ISignUpContext
    {
        Guid TenantId { get; }
        string SignUpTokenHash { get; }
    }
}
