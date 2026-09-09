namespace MiniEnv.Domain.SystemDefaults
{
    public class Permissions
    {
        public static class Names
        {
            public const string ReadDeal = "Deal.Read";
            public const string CreateDeal = "Deal.Create";
            public const string UpdateDeal = "Deal.Update";
            public const string DeleteDeal = "Deal.Delete";
            public const string ManageConfigurations = "Manage.Configurations";
            public const string ManageUsers = "Manage.Users";
        }

        private static readonly Dictionary<PermissionIndex, string> _names = new()
        {
            [PermissionIndex.ReadDeal] = Names.ReadDeal,
            [PermissionIndex.CreateDeal] = Names.CreateDeal,
            [PermissionIndex.UpdateDeal] = Names.UpdateDeal,
            [PermissionIndex.DeleteDeal] = Names.DeleteDeal,
            [PermissionIndex.ManageConfigurations] = Names.ManageConfigurations,
            [PermissionIndex.ManageUsers] = Names.ManageUsers
        };

        public static IReadOnlyDictionary<PermissionIndex, string> Collection => _names.AsReadOnly();
    }
}
