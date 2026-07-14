using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Entities;
using Shared.Domain.Events;
using Shared.Domain.Interfaces;

namespace Shared.Infrastructure.Persistence;

public abstract class BaseDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService; 
    protected BaseDbContext(DbContextOptions options, IMediator mediator, ICurrentUserService currentUserService) : base(options)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tự động tìm và áp dụng các cấu hình (Configurations) trong cùng Assembly của Service
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // 1. Trước khi lưu, tự động cập nhật thông tin Auditing (Created/Modified)
        ApplyAbstractions();

        // 2. Thực hiện lưu vào Database
        var result = await base.SaveChangesAsync(cancellationToken);

        // 3. Sau khi lưu thành công, bạn có thể Dispatch Domain Events tại đây (nếu muốn)
        await DispatchDomainEventsAsync();
        return result;
    }

    private void ApplyAbstractions()
    {
        var currentUserId = _currentUserService.UserId;

        // Quét các thực thể đang được theo dõi bởi EF Core có kế thừa IBaseEntity
        var entries = ChangeTracker.Entries<IBaseEntity>();

        foreach (var entry in entries)
        {
            // 2. Kiểm tra nếu nó thực thi IBaseEntity<Guid> hoặc bất kỳ IBaseEntity<T> nào
            // Cách an toàn nhất là kiểm tra xem nó có method UpdateAudit không thông qua interface generic hoặc reflection
            // Nhưng vì chúng ta đã thêm UpdateAudit vào interface có thể cast động:

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                // Sử dụng Reflection hoặc ép kiểu sang một interface chung có chứa UpdateAudit
                // Ở đây ta dùng 'dynamic' hoặc ép kiểu sang interface có method UpdateAudit
                if (entry.Entity is IBaseEntity entity)
                {
                    // Cách dùng Reflection để gọi method UpdateAudit trên lớp BaseEntity<TId>
                    var method = entity.GetType().GetMethod("UpdateAudit");
                    method?.Invoke(entity, new object[] { DateTime.UtcNow, currentUserId });
                }
            }
        }
    }

    private async Task DispatchDomainEventsAsync()
    {
        // Lấy tất cả các entity là AggregateRoot có chứa domain event
        var domainEntities = ChangeTracker
            .Entries<IAggregateRoot>()
            // Do IAggregateRoot là interface rỗng, bạn cần ép kiểu (cast) về AggregateRoot<T>
            // hoặc cách nhanh nhất là lấy entity, dùng dynamic/reflection, 
            // HOẶC sửa IAggregateRoot có thêm hàm IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
            .Select(x => x.Entity)
            .Where(x => x.GetType().GetProperty("DomainEvents") != null)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => (IEnumerable<IDomainEvent>)x.GetType().GetProperty("DomainEvents").GetValue(x, null))
            .ToList();

        domainEntities.ForEach(entity =>
        {
            entity.GetType().GetMethod("ClearDomainEvents").Invoke(entity, null);
        });

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
