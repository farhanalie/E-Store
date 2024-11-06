using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasVogenConversion();
        builder.Property(o => o.OrderName).HasVogenConversion();

        builder.HasMany<OrderItem>().WithOne().HasForeignKey(item => item.OrderId);
        builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId).IsRequired();
        builder.ComplexProperty(c => c.ShippingAddress, address =>
        {
            address.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            address.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            address.Property(a => a.EmailAddress).HasMaxLength(255).IsRequired();
            address.Property(a => a.AddressLine).HasMaxLength(250).IsRequired();
            address.Property(a => a.State).HasMaxLength(100).IsRequired();
            address.Property(a => a.Country).HasMaxLength(100).IsRequired();
            address.Property(a => a.ZipCode).HasMaxLength(18).IsRequired();
        });

        builder.ComplexProperty(c => c.BillingAddress, address =>
        {
            address.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            address.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            address.Property(a => a.EmailAddress).HasMaxLength(255).IsRequired();
            address.Property(a => a.AddressLine).HasMaxLength(250).IsRequired();
            address.Property(a => a.State).HasMaxLength(100).IsRequired();
            address.Property(a => a.Country).HasMaxLength(100).IsRequired();
            address.Property(a => a.ZipCode).HasMaxLength(18).IsRequired();
        });

        builder.ComplexProperty(c => c.Payment, payment =>
        {
            payment.Property(p => p.CardName).HasMaxLength(50);
            payment.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
            payment.Property(p => p.Expiration).HasMaxLength(10).IsRequired();
            payment.Property(p => p.CVV).HasMaxLength(3).IsRequired();
            payment.Property(p => p.PaymentMethod);
        });

        builder.Property(c => c.Status)
            .HasDefaultValue(OrderStatus.Draft)
            // .HasConversion<StringToEnumConverter<OrderStatus>>();
            .HasConversion(
                s => s.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(o => o.TotalPrice);
    }
}
