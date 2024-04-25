using eStoreOnline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eStoreOnline.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        
        builder.ToTable("Orders");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrderDate).IsRequired();
        builder.Property(x => x.ShippingAddress).IsRequired().HasMaxLength(500);
        builder.Property(x => x.OrderStatus).IsRequired().HasMaxLength(50);
        builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.UserId).IsRequired();
        builder.HasMany(x => x.OrderDetails).WithOne().HasForeignKey(x => x.OrderId);
    }
}