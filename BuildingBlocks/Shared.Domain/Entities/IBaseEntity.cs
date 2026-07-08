namespace Shared.Domain.Entities;

public interface IBaseEntity { }
public interface IBaseEntity<TId> : IBaseEntity
{
    TId Id { get; }
    DateTime CreatedDate { get; }
    string? CreatedBy { get; }
    DateTime? LastModifiedDate { get; }
    string? LastModifiedBy { get; }
    void UpdateAudit(DateTime now, string? userId = null);
}