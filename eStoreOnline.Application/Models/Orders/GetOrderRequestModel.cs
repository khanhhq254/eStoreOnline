namespace eStoreOnline.Application.Models.Orders;

public class GetOrderRequestModel : BasePaginatedRequest
{
    public string? UserId { get; set; }
}