using MiniEnv.Domain.Entities;
using MiniEnv.Domain.Enums;

namespace MiniEnv.Domain.SystemDefaults
{
    public class DefaultRoles
    {
        private static readonly Dictionary<RoleIndex, string> _names = new()
        {
            [RoleIndex.Owner] = "Owner",
            [RoleIndex.Employee] = "Employee"
        };

        private static readonly Dictionary<RoleIndex, IReadOnlyCollection<PermissionAccess>> _roles = new()
        {
            [RoleIndex.Owner] = [
                new PermissionAccess(PermissionIndex.ReadDeal, AccessLevel.All),
                new PermissionAccess(PermissionIndex.CreateDeal, AccessLevel.All),
                new PermissionAccess(PermissionIndex.UpdateDeal, AccessLevel.All),
                new PermissionAccess(PermissionIndex.DeleteDeal, AccessLevel.All),
                new PermissionAccess(PermissionIndex.ManageConfigurations, AccessLevel.All),
                new PermissionAccess(PermissionIndex.ManageUsers, AccessLevel.All)
                ],
            [RoleIndex.Employee] = [
                new PermissionAccess(PermissionIndex.ReadDeal, AccessLevel.Own),
                new PermissionAccess(PermissionIndex.CreateDeal, AccessLevel.Own),
                new PermissionAccess(PermissionIndex.UpdateDeal, AccessLevel.Own),
                new PermissionAccess(PermissionIndex.DeleteDeal, AccessLevel.None),
                new PermissionAccess(PermissionIndex.ManageConfigurations, AccessLevel.None),
                new PermissionAccess(PermissionIndex.ManageUsers, AccessLevel.None)
            ]
        };

        public static IReadOnlyDictionary<RoleIndex, string> RoleNames => _names;
        public static IReadOnlyDictionary<RoleIndex, IReadOnlyCollection<PermissionAccess>> Collection => _roles.AsReadOnly();

        public static IReadOnlyDictionary<RoleIndex, Role> CreateForTenant(
            IReadOnlyDictionary<PermissionIndex, Guid> permissionIds)
        {
            var roles = new Dictionary<RoleIndex, Role>();

            foreach (var (roleIndex, permissionAccesses) in _roles)
            {
                Role role = Role.Create(
                    _names[roleIndex],
                    EntityOrigin.System);

                Dictionary<Guid, AccessLevel> permissions = permissionAccesses.ToDictionary(
                    permissionAccess => permissionIds[permissionAccess.Permission],
                    permissionAccess => permissionAccess.AccessLevel);

                role.SetPermissions(permissions);

                roles.Add(roleIndex, role);
            }

            return roles;
        }
    }
}
