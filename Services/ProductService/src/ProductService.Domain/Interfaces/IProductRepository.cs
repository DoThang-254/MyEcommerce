using ProductService.Domain.Entities;
using Shared.Domain.Interfaces;

namespace ProductService.Domain.Interfaces;

public interface IProductRepository : IBaseRepository<Product, Guid>
{
    Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId);

}

