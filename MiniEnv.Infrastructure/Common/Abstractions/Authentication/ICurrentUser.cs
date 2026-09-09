using MiniEnv.Domain.Enums;

namespace MiniEnv.Infrastructure.Common.Abstractions.Authentication
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        Guid TenantId { get; }
        string Email { get; }
        UserPermissions Permissions { get; }

        Task LoadPermissions(CancellationToken cancellationToken);
    }
    public sealed record UserPermissions(
        Guid RoleId,
        IReadOnlyDictionary<string, AccessLevel> PermissionsAccessLevel
    );

}
