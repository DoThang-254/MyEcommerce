namespace Shared.Domain.Entities;

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    public TId Id { get; protected set; } = default!; // Tự động tạo Id mới

    public DateTime CreatedDate { get; private set; }
    public string? CreatedBy { get; protected set; }
    public DateTime? LastModifiedDate { get; private set; }
    public string? LastModifiedBy { get; private set; }

    // Dùng nội bộ cho DbContext cập nhật tự động
    public void UpdateAudit(DateTime now, string? userId = null)
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
