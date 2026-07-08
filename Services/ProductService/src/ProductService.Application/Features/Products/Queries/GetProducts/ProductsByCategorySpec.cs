using Ardalis.Specification;
using ProductService.Domain.Entities;

namespace ProductService.Application.Features.Products.Queries.GetProducts
{
    public class ProductsByCategorySpec : Specification<Product>
    {
        public ProductsByCategorySpec(GetProductsQuery request)
        {
            // Điều kiện 1: Lọc theo Category
            if (request.CategoryId.HasValue)
            {
                Query.Where(p => p.CategoryId == request.CategoryId.Value);
            }

            // Điều kiện 2: Lọc thêm theo tên (Nếu có truyền searchTerm)
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                Query.Where(p => p.Name.Contains(request.SearchTerm));
            }

            // Có thể thêm OrderBy luôn nếu muốn
            Query.OrderByDescending(p => p.CreatedDate);

            int validPageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            int validPageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            if (validPageSize > 100) validPageSize = 100;

            Query.Skip((validPageIndex - 1) * validPageSize)
                 .Take(validPageSize);
        }
    }
}
