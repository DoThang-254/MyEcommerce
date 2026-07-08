using Ardalis.Specification;

namespace Shared.Domain.Interfaces
{
    public interface IBaseRepository<TEntity, TId> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
        Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
