namespace eStoreOnline.Domain.Entities;

public class OrderDetail : BaseEntity
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}