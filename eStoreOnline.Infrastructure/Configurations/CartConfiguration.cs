using eStoreOnline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eStoreOnline.Infrastructure.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.HasMany(x => x.CartDetails).WithOne().HasForeignKey(x => x.CartId);
    }
}