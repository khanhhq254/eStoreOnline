namespace eStoreOnline.Application.Models.Orders;

public class OrderItemmodel
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
}