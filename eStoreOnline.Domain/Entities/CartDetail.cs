using System.ComponentModel.DataAnnotations.Schema;

namespace eStoreOnline.Domain.Entities;

public class CartDetail : BaseEntity
{
    public int Id { get; set; }
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int CartId { get; set; }

    [ForeignKey(nameof(CartId))]
    public Cart Cart { get; set; } = default!;
}