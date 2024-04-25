using eStoreOnline.Application.Models.Carts;

namespace eStoreOnline.Application.Interfaces;

public interface ICartService
{
    Task AddItemToCartAsync(AddItemToCartModel model);
    Task RemoveItemFromCartAsync(RemoveItemFromCartModel model);
    Task<CartModel> GetCartByUserAsync(CartRequestModel request);
    Task<CartModel> UpdateCartAsync(UpdateCartRequestModel request);
}