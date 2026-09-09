namespace MiniEnv.Application.Features.Users.RevokeInvitation
{
    public sealed record RevokeInvitationCommand(
        Guid InvitationId);
}
