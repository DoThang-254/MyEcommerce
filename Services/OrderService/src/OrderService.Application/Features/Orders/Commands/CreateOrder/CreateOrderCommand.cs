using Shared.Application.Features.Messaging;

namespace OrderService.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(
        Guid CustomerId,
        string CustomerName,       
        string CustomerEmail,      
        string CustomerPhone,      
        AddressDto ShippingAddress, 
        List<OrderItemDto> OrderItems
    ) : ICommand<Guid>;

    public record OrderItemDto(
            Guid ProductId,
            string ProductName,
            string? ProductImage,
            decimal Price,
            int Quantity
        );
    // Cấu trúc địa chỉ chi tiết để map vào Value Object Address của Domain
    public record AddressDto(string Street, string City, string State, string Country, string ZipCode);

}
