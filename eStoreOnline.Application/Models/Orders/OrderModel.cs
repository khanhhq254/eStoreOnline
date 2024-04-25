using eStoreOnline.Application.Models.Carts;

namespace eStoreOnline.Application.Models.Orders;

public class OrderModel
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
}