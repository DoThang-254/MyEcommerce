using Shared.Application.Features.Messaging;

namespace OrderService.Application.Features.Orders.Commands.UpdateOrder
{
    public record CancelOrderCommand(Guid OrderId, string Reason) : ICommand<bool>;
}
