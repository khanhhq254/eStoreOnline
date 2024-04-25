using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Carts;
using eStoreOnline.Application.Models.Orders;
using eStoreOnline.Data;
using eStoreOnline.Domain.Entities;
using eStoreOnline.Domain.Enums;
using eStoreOnline.Domain.Exceptions;
using eStoreOnline.Infrastructure.Consts;
using eStoreOnline.Infrastructure.Interfaces;
using eStoreOnline.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace eStoreOnline.Application.Implementations;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IPaymentGatewayService _paymentGatewayService;

    public OrderService(ApplicationDbContext context, IPaymentGatewayService paymentGatewayService)
    {
        _context = context;
        _paymentGatewayService = paymentGatewayService;
    }

    public async Task CreateOrderAsync(CreateOrderRequestModel request)
    {
        var cart = await _context.Carts
            .Include(x => x.CartDetails)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == request.CartId && x.UserId == request.UserId);

        if (cart == null)
        {
            throw new NotFoundException(nameof(Cart), request.CartId.ToString());
        }

        var order = new Order
        {
            UserId = request.UserId,
            TotalAmount = cart.TotalPrice,
            OrderStatus = OrderStatus.Pending,
            OrderNumber = GenerateOrderNumber(),
            OrderDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        foreach (var cartItem in cart.CartDetails)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            });
        }

        var session = await _paymentGatewayService.CreatePaymentSessionAsync(new CreatePaymentSessionModel()
        {
            Domain = request.Domain,
            OrderNumber = order.OrderNumber,
            Items = cart.CartDetails.Select(x => new CreatePaymentItemModel()
            {
                ProductName = x.Product.ProductName,
                Price = x.Product.Price,
                Quantity = x.Quantity
            }).ToList()
        });

        if (session == null) throw new Exception("Can not create order");

        order.StripeSessionId = session.StripeSessionId;

        _context.CartDetails.RemoveRange(cart.CartDetails);
        _context.Carts.Remove(cart);
        _context.Orders.Add(order);

        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedModel<OrderModel>> GetOrdersAsync(GetOrderRequestModel request)
    {
        var orders = await _context.Orders
            .Where(x => request.UserId.HasValue && x.UserId == request.UserId)
            .Take(request.PageSize).Skip(request.PageIndex * request.PageSize).ToListAsync();

        var total = await _context.Orders.CountAsync(x => request.UserId.HasValue && x.UserId == request.UserId);

        var orderData = orders.Select(x => new OrderModel
        {
            Id = x.Id,
            OrderNumber = x.OrderNumber,
            TotalPrice = x.TotalAmount,
            OrderDate = x.OrderDate,
        }).ToList();

        return PaginatedModel<OrderModel>.Success(orderData, total, request.PageIndex, request.PageSize);
    }

    public async Task<OrderDetailModel> GetOrderDetail(GetOrderDetailRequestModel request)
    {
        var order = await _context.Orders
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Where(x => request.UserId.HasValue && x.UserId == request.UserId)
            .Where(x => x.Id == request.OrderId)
            .Take(request.PageSize)
            .Skip(request.PageIndex * request.PageSize)
            .FirstOrDefaultAsync();

        if (order == null)
            throw new NotFoundException(nameof(Order), request.OrderId);

        return new OrderDetailModel()
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            OrderNumber = order.OrderNumber,
            OrderStatus = order.OrderStatus,
            TotalAmount = order.TotalAmount,
            OrderItems = order.OrderDetails.Select(x => new OrderItemmodel()
            {
                ProductName = x.Product.ProductName,
                Price = x.Price,
                Quantity = x.Quantity,
                ImageUrl = x.Product.ImageUrl,
            }).ToList(),
        };
    }

    private string GenerateOrderNumber()
    {
        return OrderConstants.OrderNumberSuffix + DateTime.Now.ToString("yyyyMMddHHmmss");
    }
}