using System.ComponentModel.DataAnnotations;
using eStoreOnline.Domain.Enums;

namespace eStoreOnline.Application.Models.Orders;

public class OrderDetailModel
{
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus OrderStatus { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public List<OrderItemmodel> OrderItems { get; set; } = [];
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
}