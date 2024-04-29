namespace eStoreOnline.Application.Models.Orders;

public class CreateOrderResponseModel
{
    public string StripeSessionUrl { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string StripeSessionId { get; set; } = string.Empty;
}