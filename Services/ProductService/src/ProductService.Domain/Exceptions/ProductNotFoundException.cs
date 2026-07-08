using Shared.Domain.Exceptions;

namespace ProductService.Domain.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(string name, object key) : base(name, key)
        {
        }

        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}
