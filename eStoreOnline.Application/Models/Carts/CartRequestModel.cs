namespace eStoreOnline.Application.Models.Carts;

public class CartRequestModel
{
    public string UserId { get; set; } = string.Empty;
    public int CartId { get; set; }
}