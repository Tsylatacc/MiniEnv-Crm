namespace MiniEnv.Application.Features.Customers.Patch
{
    public sealed record PatchCustomerCommand(
        Guid CustomerId,
        string? Name,
        string? PhoneNumber,
        string? Email);
}
