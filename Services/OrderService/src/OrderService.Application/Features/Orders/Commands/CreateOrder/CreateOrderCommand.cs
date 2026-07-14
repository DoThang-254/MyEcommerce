using Shared.Application.Features.Messaging;

namespace OrderService.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(
        //Guid CustomerId,
        string CustomerName,       // Bổ sung để nạp vào Order Constructor
        string CustomerEmail,      // Bổ sung để tạo Value Object EmailAddress
        string CustomerPhone,      // Bổ sung để tạo Value Object PhoneNumber
        AddressDto ShippingAddress, // Dùng Dto cấu trúc thay vì chuỗi phẳng để dễ map vào Value Object Address
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
