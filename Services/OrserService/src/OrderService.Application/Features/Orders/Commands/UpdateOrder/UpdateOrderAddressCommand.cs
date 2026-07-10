using Shared.Application.Features.Messaging;

namespace OrderService.Application.Features.Orders.Commands.UpdateOrder
{
    public record UpdateOrderAddressCommand(
         Guid OrderId,
         string Street,
         string City,
         string State,
         string Country,
         string ZipCode
     ) : ICommand<bool>;
}
