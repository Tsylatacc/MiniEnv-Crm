namespace MiniEnv.Infrastructure.Common.Abstractions.Authentication
{
    public interface IRefreshContext
    {
        string RefreshToken { get; }
    }
}