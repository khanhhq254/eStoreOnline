namespace eStoreOnline.Application.Models.Carts;

public class CartDetailModel
{
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string ProductName { get; set; }
    public string ShortDescription { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }
}