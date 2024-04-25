using eStoreOnline.Application.Models.Carts;

namespace eStoreOnline.Application.Interfaces;

public interface ICartService
{
    Task AddItemToCartAsync(int productId, int quantity);
    Task RemoveItemFromCartAsync(int productId);
    Task<CartModel> GetCartByUserAsync(CartRequestModel request);
    Task<CartModel> UpdateCartAsync(UpdateCartRequestModel request);
}