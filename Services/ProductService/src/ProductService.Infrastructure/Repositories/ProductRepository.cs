using BuildingBlocks.Shared.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product, Guid, ProductDbContext>, IProductRepository
    {
        public ProductRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _dbContext.Set<Product>()
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
        }
    }
}
