namespace eStoreOnline.Application.Models.Carts;

public class RemoveItemFromCartModel
{
    public int CartId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ProductId { get; set; }
}