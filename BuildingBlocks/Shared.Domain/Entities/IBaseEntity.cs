namespace Shared.Domain.Entities;

public interface IBaseEntity { }
public interface IBaseEntity<TId> : IBaseEntity
{
    TId Id { get; }
    DateTime CreatedDate { get; }
    Guid? CreatedBy { get; }
    DateTime? LastModifiedDate { get; }
    Guid? LastModifiedBy { get; }
    void UpdateAudit(DateTime now, Guid? userId = null);
}