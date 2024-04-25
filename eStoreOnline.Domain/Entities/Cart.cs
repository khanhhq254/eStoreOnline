using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace eStoreOnline.Domain.Entities;

public class Cart : BaseEntity
{
    public int Id { get; set; }
    public List<CartDetail> CartDetails { get; set; } = new();
    public decimal TotalPrice { get; set; }

    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; } = default!;
}