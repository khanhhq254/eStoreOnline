namespace eStoreOnline.Application.Models.Carts;

public class UpdateCartRequestModel
{
    public int CartId { get; set; }
    public List<UpdateCartDetailRequestModel> CartDetails { get; set; } = [];
    public string UserId { get; set; } = string.Empty;
}