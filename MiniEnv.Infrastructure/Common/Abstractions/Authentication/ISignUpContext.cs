namespace MiniEnv.Infrastructure.Common.Abstractions.Authentication
{
    public interface ISignUpContext
    {
        Guid TenantId { get; }
        Guid SignUpId { get; }
        string Email { get; }
    }
}
