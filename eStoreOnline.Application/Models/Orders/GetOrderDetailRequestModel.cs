namespace eStoreOnline.Application.Models.Orders;

public class GetOrderDetailRequestModel : BasePaginatedRequest
{
    public string? UserId { get; set; }
    public int OrderId { get; set; }
}