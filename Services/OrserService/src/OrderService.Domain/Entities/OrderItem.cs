using Shared.Domain.ValueObjects;
using System;

namespace OrderService.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderItemId { get; private set; }
        public Guid OrderId { get; private set; }

        // Snapshot Product
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public string? ProductImage { get; private set; }

        // Sử dụng Value Object Money thay cho decimal
        public Money UnitPrice { get; private set; } = null!;
        public int Quantity { get; private set; }
        public Money TotalPrice { get; private set; } = null!;

        // Navigation
        public Order Order { get; private set; } = null!;

        // Constructor rỗng (Bắt buộc cho Entity Framework Core khi load từ DB)
        private OrderItem() { }

        // Constructor chỉ dùng trong nội bộ AggregateRoot
        internal OrderItem(Guid productId, string productName, string? productImage, Money unitPrice, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Số lượng phải lớn hơn 0");

            OrderItemId = Guid.NewGuid();
            ProductId = productId;
            ProductName = string.IsNullOrWhiteSpace(productName) ? throw new ArgumentException("Tên sản phẩm không được trống") : productName;
            ProductImage = productImage;
            UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
            Quantity = quantity;

            // Tính toán giá trực tiếp khi tạo mới item
            TotalPrice = Money.Of(unitPrice.Amount * quantity, unitPrice.Currency);
        }
    }
}