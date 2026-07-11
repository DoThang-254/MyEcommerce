using OrderService.Domain.Entities;
using Shared.Domain.Interfaces;

namespace OrderService.Domain.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order , Guid>
    {

    }
}
