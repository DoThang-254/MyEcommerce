namespace Shared.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? CorrelationId { get; }
        string? IpAddress { get; }
    }
}
