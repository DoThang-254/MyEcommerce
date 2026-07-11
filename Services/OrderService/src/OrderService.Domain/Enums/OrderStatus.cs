namespace OrderService.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 0,          // Chờ xác nhận

        Confirmed = 1,        // Đã xác nhận

        Processing = 2,       // Đang chuẩn bị hàng

        Shipping = 3,         // Đang giao

        Delivered = 4,        // Đã giao

        Cancelled = 5,        // Đã hủy

        Refunded = 6          // Hoàn tiền
    }
}