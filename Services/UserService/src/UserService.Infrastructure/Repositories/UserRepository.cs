using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using BuildingBlocks.Shared.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.ValueObjects;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User, Guid, UserDbContext>, IUserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<User>().AddAsync(entity, cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<User> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).CountAsync(cancellationToken);
        }

        public void Delete(User entity)
        {
            _dbContext.Set<User>().Remove(entity);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var emailVo = EmailAddress.Create(email);
            
            return await _dbContext.Set<User>()
                .FirstOrDefaultAsync(u => u.Email == emailVo, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> ListAsync(ISpecification<User> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        public void Update(User entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        // Helper để áp dụng Specification (Ardalis.Specification)
        private IQueryable<User> ApplySpecification(ISpecification<User> specification)
        {
            return SpecificationEvaluator.Default.GetQuery(_dbContext.Set<User>().AsQueryable(), specification);
        }
    }
}