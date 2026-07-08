using Microsoft.EntityFrameworkCore;
using Shared.Domain.Interfaces;

namespace Shared.Infrastructure.Repositories;
public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;

    public UnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
