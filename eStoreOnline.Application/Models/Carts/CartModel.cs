using System.Collections.Immutable;

namespace eStoreOnline.Application.Models.Carts;

public class CartModel
{
    public int CartId { get; set; }
    public ImmutableList<CartDetailModel> CartDetail { get; set; } = [];
    public decimal TotalAmount => CartDetail.Sum(x => x.Amount);
}