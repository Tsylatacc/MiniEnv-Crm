namespace MiniEnv.Infrastructure.Common.Exceptions
{
    public sealed class ForbiddenException : Exception
    {
        public ForbiddenException() : base("Access denied for this resource.") { }
    }
}
