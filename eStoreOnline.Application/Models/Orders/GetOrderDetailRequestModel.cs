namespace eStoreOnline.Application.Models.Orders;

public class GetOrderDetailRequestModel
{
    public string? UserId { get; set; }
    public int? OrderId { get; set; }
    public string? OrderNumber { get; set; }
}