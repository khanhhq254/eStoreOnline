namespace eStoreOnline.Domain.Entities;

public class CartDetail :BaseEntity
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; }
    
}