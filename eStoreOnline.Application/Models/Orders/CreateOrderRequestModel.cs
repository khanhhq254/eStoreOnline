namespace eStoreOnline.Application.Models.Orders;

public class CreateOrderRequestModel
{
    public string Domain { get; set; }
    public int CartId { get; set; }
    public string UserId { get; set; } = string.Empty;
}