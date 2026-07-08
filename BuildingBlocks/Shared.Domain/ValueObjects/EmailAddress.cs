using System.Text.RegularExpressions;

namespace Shared.Domain.ValueObjects;

public record EmailAddress
{
    public string Value { get; }

    private EmailAddress(string value) => Value = value;

    public static EmailAddress Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email không được để trống");

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Định dạng Email không hợp lệ");

        return new EmailAddress(email);
    }
}
