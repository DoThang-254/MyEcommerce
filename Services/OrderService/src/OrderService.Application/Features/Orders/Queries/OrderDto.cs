namespace OrderService.Application.Features.Orders.Queries
{
    public record OrderDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string CustomerName { get; init; } = string.Empty;

        // Flatten (trải phẳng) Value Object Money ra để Client dễ đọc
        public decimal TotalAmount { get; init; }

        public string Status { get; init; } = string.Empty;
        public string PaymentMethod { get; init; } = string.Empty;
        public bool IsPaid { get; init; }

        public DateTime CreatedDate { get; init; }
    }
}
