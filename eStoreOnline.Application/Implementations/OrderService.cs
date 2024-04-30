using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Carts;
using eStoreOnline.Application.Models.Orders;
using eStoreOnline.Domain.Entities;
using eStoreOnline.Domain.Enums;
using eStoreOnline.Domain.Exceptions;
using eStoreOnline.Infrastructure.Consts;
using eStoreOnline.Infrastructure.Data;
using eStoreOnline.Infrastructure.Interfaces;
using eStoreOnline.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using StripeConfiguration = eStoreOnline.Infrastructure.Configurations.StripeConfiguration;

namespace eStoreOnline.Application.Implementations;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly StripeConfiguration _options;

    public OrderService(ApplicationDbContext context, IPaymentGatewayService paymentGatewayService, IOptions<StripeConfiguration> options)
    {
        _context = context;
        _paymentGatewayService = paymentGatewayService;
        _options = options.Value;
    }

    public async Task<CreateOrderResponseModel> CreateOrderAsync(CreateOrderRequestModel request)
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
            ModifiedDate = DateTime.UtcNow,
            CreatedBy = request.UserId,
            ModifiedBy = request.UserId
        };

        foreach (var cartItem in cart.CartDetails)
        {
            order.OrderDetails.Add(new OrderDetail
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreatedBy = request.UserId,
                ModifiedBy = request.UserId
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

        return new CreateOrderResponseModel()
        {
            StripeSessionId = session.StripeSessionId,
            OrderNumber = order.OrderNumber,
            StripeSessionUrl = session.StripeSessionUrl
        };
    }

    public async Task<PaginatedModel<OrderModel>> GetOrdersAsync(GetOrderRequestModel request)
    {
        var orders = await _context.Orders
            .Where(x => !string.IsNullOrWhiteSpace(request.UserId) && x.UserId == request.UserId)
            .Take(request.PageSize).Skip(request.PageIndex * request.PageSize).ToListAsync();

        var total = await _context.Orders.CountAsync(x =>
            !string.IsNullOrWhiteSpace(request.UserId) && x.UserId == request.UserId);

        var orderData = orders.Select(x => new OrderModel
        {
            Id = x.Id,
            OrderNumber = x.OrderNumber,
            TotalPrice = x.TotalAmount,
            OrderDate = x.OrderDate,
        }).ToList();

        return PaginatedModel<OrderModel>.Success(orderData, total, request.PageIndex, request.PageSize);
    }

    public async Task<OrderDetailModel> GetOrderDetailAsync(GetOrderDetailRequestModel request)
    {
        var order = await _context.Orders
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Where(x => !string.IsNullOrWhiteSpace(request.UserId) && x.UserId == request.UserId)
            .Where(x => (request.OrderId.HasValue && x.Id == request.OrderId) ||
                        (!string.IsNullOrWhiteSpace(request.OrderNumber) && x.OrderNumber == request.OrderNumber))
            .FirstOrDefaultAsync();

        var orderKey = request.OrderId.HasValue ? request.OrderId.ToString() : request.OrderNumber;

        if (order == null)
            throw new NotFoundException(nameof(Order), orderKey + "");

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

    public async Task<bool> OrderProcessingAsync(OrderProcessingRequestModel request)
    {
        var stripeEvent = EventUtility.ConstructEvent(
            request.BodyContent,
            request.Signature,
            _options.WebhookSecret
        );
        
        if (stripeEvent.Data.Object is not Session session)
            return false;
        
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.StripeSessionId == session.Id);
        
        if (order == null)
            return false;

        if (order.OrderStatus == OrderStatus.Completed)
            return true;

        order.OrderStatus = stripeEvent.Type switch
        {
            Events.PaymentIntentSucceeded => OrderStatus.Pending,
            Events.PaymentIntentCanceled => OrderStatus.Cancelled,
            Events.CheckoutSessionCompleted => OrderStatus.Completed,
            Events.CheckoutSessionExpired => OrderStatus.Cancelled,
            _ => order.OrderStatus
        };

        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateOrderNumber()
    {
        return OrderConstants.OrderNumberSuffix + DateTime.Now.ToString("yyyyMMddHHmmss");
    }
}