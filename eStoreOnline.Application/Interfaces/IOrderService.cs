using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Carts;
using eStoreOnline.Application.Models.Orders;

namespace eStoreOnline.Application.Interfaces;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrderRequestModel request);
    Task<PaginatedModel<CartModel>> GetOrdersAsync(GetOrderRequestModel request);
    Task<OrderDetailModel> GetOrderDetail(int orderId);
}