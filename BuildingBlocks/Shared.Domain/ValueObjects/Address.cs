namespace Shared.Domain.ValueObjects;

public record Address
{
    public string Street { get; init; } = default!;
    public string City { get; init; } = default!;
    public string State { get; init; } = default!;
    public string Country { get; init; } = default!;
    public string ZipCode { get; init; } = default!;

    // Constructor bắt buộc phải có đầy đủ thông tin
    public Address(string street, string city, string state, string country, string zipCode)
    {
        // Kiểm tra validation cơ bản
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street is required", nameof(street));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required", nameof(city));
        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required", nameof(country));

        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    // Constructor rỗng dành cho Entity Framework Core (nếu cần)
    private Address() { }

    // Static factory để tạo nhanh một địa chỉ trống/mặc định nếu cần
    public static Address Empty => new("N/A", "N/A", "N/A", "N/A", "N/A");

    // Hiển thị địa chỉ theo định dạng chuẩn khi in ra log hoặc UI
    public override string ToString()
        => $"{Street}, {City}, {State}, {ZipCode}, {Country}";
}
