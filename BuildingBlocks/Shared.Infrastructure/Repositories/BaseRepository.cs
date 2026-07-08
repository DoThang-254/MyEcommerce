using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Interfaces;

namespace BuildingBlocks.Shared.Infrastructure.Repositories;

public class BaseRepository<TEntity, TId, TContext> : IBaseRepository<TEntity, TId>
    where TEntity : class
    where TContext : DbContext
{
    protected readonly TContext _dbContext;

    public BaseRepository(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        // FindAsync nhận một mảng params object
        return await _dbContext.Set<TEntity>().FindAsync(new object[] { id! }, cancellationToken);
    }


    public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification, evaluateCriteriaOnly: true).CountAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false)
    {
        return SpecificationEvaluator.Default.GetQuery(
            query: _dbContext.Set<TEntity>().AsQueryable(),
            specification: specification,
            evaluateCriteriaOnly: evaluateCriteriaOnly);
    }
}