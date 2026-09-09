namespace MiniEnv.Application.Features.Users.Invite
{
    public sealed record InviteCommand(
        string Name,
        string Email,
        Guid? RoleId);
}
