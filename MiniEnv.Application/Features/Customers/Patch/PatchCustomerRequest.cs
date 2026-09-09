namespace MiniEnv.Application.Features.Customers.Patch
{
    public sealed record PatchCustomerRequest(
        string? Name,
        string? PhoneNumber,
        string? Email);
}
