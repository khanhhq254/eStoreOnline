using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Carts;
using eStoreOnline.Application.Models.Orders;

namespace eStoreOnline.Application.Interfaces;

public interface IOrderService
{
    Task<CreateOrderResponseModel> CreateOrderAsync(CreateOrderRequestModel request);
    Task<PaginatedModel<OrderModel>> GetOrdersAsync(GetOrderRequestModel request);
    Task<OrderDetailModel> GetOrderDetailAsync(GetOrderDetailRequestModel request);
    Task<bool> OrderProcessingAsync(OrderProcessingRequestModel request);
}