namespace eStoreOnline.Application.Models.Orders;

public class OrderProcessingRequestModel
{
    public string BodyContent { get; set; } = string.Empty;
    
    public string? Signature { get; set; }
}