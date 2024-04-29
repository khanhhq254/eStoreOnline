namespace eStoreOnline.Application.Models.Carts;

public class AddItemToCartModel
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string UserId { get; set; } = string.Empty;
}