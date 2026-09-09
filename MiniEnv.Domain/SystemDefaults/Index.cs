namespace MiniEnv.Domain.SystemDefaults
{
    public enum PermissionIndex
    {
        ReadDeal,
        CreateDeal,
        UpdateDeal,
        DeleteDeal,
        ManageConfigurations,
        ManageUsers
    }

    public enum RoleIndex
    {
        Owner,
        Employee
    }

    public enum PipelineIndex
    {
        Sales
    }

    public enum StageIndex
    {
        Lead,
        Qualified,
        Proposal,
        Negotiation,
        Won,
        Lost
    }
}
