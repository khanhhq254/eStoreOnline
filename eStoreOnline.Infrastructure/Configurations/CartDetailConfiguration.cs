using eStoreOnline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eStoreOnline.Infrastructure.Configurations;

public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
{
    public void Configure(EntityTypeBuilder<CartDetail> builder)
    {
        builder.ToTable("CartDetails");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .ValueGeneratedOnAdd();

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.Price)
            .IsRequired();

        builder.Property(ci => ci.ProductId)
            .IsRequired();

        builder.Property(ci => ci.CartId)
            .IsRequired();

        builder.HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId);

        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.CartDetails)
            .HasForeignKey(ci => ci.CartId);
    }
}