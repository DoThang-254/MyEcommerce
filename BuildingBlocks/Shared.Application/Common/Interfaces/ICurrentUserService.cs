namespace Shared.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string? CorrelationId { get; }
        string? IpAddress { get; }
    }
}
