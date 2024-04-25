namespace eStoreOnline.Application.Models.Carts;

public class RemoveItemFromCartModel
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
}