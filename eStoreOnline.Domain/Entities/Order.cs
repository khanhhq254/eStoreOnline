using System.ComponentModel.DataAnnotations.Schema;
using eStoreOnline.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace eStoreOnline.Domain.Entities;

public class Order : BaseEntity
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public OrderStatus OrderStatus { get; set; }
    public decimal TotalAmount { get; set; }

    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; } = default!;

    public List<OrderDetail> OrderDetails { get; set; } = [];
    public string OrderNumber { get; set; } = string.Empty;
    public string StripeSessionId { get; set; } = string.Empty;
}