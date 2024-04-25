using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Carts;
using eStoreOnline.Application.Models.Orders;

namespace eStoreOnline.Application.Implementations;

public class OrderService : IOrderService
{
    public Task CreateOrderAsync(CreateOrderRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedModel<CartModel>> GetOrdersAsync(GetOrderRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task<OrderDetailModel> GetOrderDetail(int orderId)
    {
        throw new NotImplementedException();
    }
}