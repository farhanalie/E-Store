using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasVogenConversion();
        builder.Property(x => x.OrderId).IsRequired();
        builder.HasOne<Product>().WithMany().HasForeignKey(oi => oi.ProductId).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
    }
}
