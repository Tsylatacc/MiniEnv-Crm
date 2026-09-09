namespace MiniEnv.Application.Features.Users.AcceptInvitation
{
    public sealed record AcceptInvitationCommand(
        string InvitationToken,
        string Password);
}
