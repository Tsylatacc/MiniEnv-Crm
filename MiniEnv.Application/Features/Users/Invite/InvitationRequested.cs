namespace MiniEnv.Application.Features.Users.Invite
{
    public sealed record InvitationRequested(
        Guid InvitationId,
        string Email,
        string InvitationToken);
}
