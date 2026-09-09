namespace MiniEnv.Application.Features.Customers.Create
{
    public sealed record CreateCustomerCommand(
        string Name,
        string? PhoneNumber,
        string? Email);
}
