namespace eStoreOnline.Application.Models.Carts;

public class UpdateCartRequestModel
{
    public int CartId { get; set; }
    public List<UpdateCartDetailRequestModel> CartDetails { get; set; } = [];
    public int UserId { get; set; }
}