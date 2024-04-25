namespace eStoreOnline.Application.Models.Orders;

public class GetOrderRequestModel : BasePaginatedRequest
{
    public int? UserId { get; set; }
}