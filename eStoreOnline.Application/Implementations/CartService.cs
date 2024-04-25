using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Carts;

namespace eStoreOnline.Application.Implementations;

public class CartService : ICartService
{
    public Task AddItemToCartAsync(int productId, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task RemoveItemFromCartAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<CartModel> GetCartByUserAsync(CartRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task<CartModel> UpdateCartAsync(UpdateCartRequestModel request)
    {
        throw new NotImplementedException();
    }
}