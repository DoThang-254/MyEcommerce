namespace Shared.Domain.Entities;

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    public TId Id { get; protected set; } = default!; // Tự động tạo Id mới

    public DateTime CreatedDate { get; private set; }
    public Guid? CreatedBy { get; protected set; }
    public DateTime? LastModifiedDate { get; private set; }
    public Guid? LastModifiedBy { get; private set; }

    // Dùng nội bộ cho DbContext cập nhật tự động
    public void UpdateAudit(DateTime now, Guid? userId = null)
    {
        if (CreatedDate == default)
        {
            CreatedDate = now;
            CreatedBy = userId;
        }
        LastModifiedDate = now;
        LastModifiedBy = userId;
    }
}
