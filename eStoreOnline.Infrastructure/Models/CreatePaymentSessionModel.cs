namespace eStoreOnline.Infrastructure.Models;

public class CreatePaymentResponse
{
    public string StripeSessionId { get; set; } = null!;
    public string StripeSessionUrl { get; set; } = null!;
}

public class CreatePaymentSessionModel
{
    public string Domain { get; set; } = null!;
    public string OrderNumber { get; set; }= null!;
    public List<CreatePaymentItemModel> Items { get; set; } = [];
}

public class CreatePaymentItemModel
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ProductName { get; set; } = string.Empty;
}