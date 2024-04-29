namespace eStoreOnline.Application.Models.Carts;

public class UpdateCartRequestModel
{
    public List<UpdateCartDetailRequestModel> CartDetails { get; set; } = [];
    public string UserId { get; set; } = string.Empty;
}