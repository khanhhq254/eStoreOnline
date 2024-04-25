namespace eStoreOnline.Application.Models.Orders;

public class GetOrderDetailRequestModel : BasePaginatedRequest
{
    public int? UserId { get; set; }
    public int OrderId { get; set; }
}