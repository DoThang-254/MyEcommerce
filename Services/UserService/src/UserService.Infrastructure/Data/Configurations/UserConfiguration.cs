using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;
using Shared.Infrastructure.Persistence;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User, Guid>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("Users");

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .HasConversion(v => v.Value, v => EmailAddress.Create(v))
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PhoneNumber)
            .HasConversion(v => v.Value, v => PhoneNumber.Create(v))
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(100);

        // Cấu hình cột Role trực tiếp trong bảng Users
        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(50);
    }
}