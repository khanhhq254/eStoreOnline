using eStoreOnline.Domain.Enums;

namespace eStoreOnline.Domain.Entities;

public class Order : BaseEntity
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public OrderStatus OrderStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public int UserId { get; set; }
    // public User User { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = [];
    public string OrderNumber { get; set; } = string.Empty;
    public string StripeSessionId { get; set; } = string.Empty;
}