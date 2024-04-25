using eStoreOnline.Infrastructure.Consts;
using eStoreOnline.Infrastructure.Interfaces;
using eStoreOnline.Infrastructure.Models;
using Stripe.Checkout;

namespace eStoreOnline.Infrastructure.Implementations;

public class StripePaymentGatewayService : IPaymentGatewayService
{
    public async Task<CreatePaymentResponse> CreatePaymentSessionAsync(CreatePaymentSessionModel request)
    {
        var lineItems = request.Items.Select(cartItem => new SessionLineItemOptions()
        {
            PriceData = new SessionLineItemPriceDataOptions()
            {
                UnitAmountDecimal = cartItem.Price,
                Currency = OrderConstants.DefaultCurrency,
                ProductData = new SessionLineItemPriceDataProductDataOptions()
                {
                    Name = cartItem.ProductName,
                },
            },
            Quantity = cartItem.Quantity,
        }).ToList();
        
        var stripeSessionOption = new SessionCreateOptions
        {
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = $"{request.Domain}/Cart/Complete/{request.OrderNumber}",
            CancelUrl = $"{request.Domain}/Cart/Cancel/{request.OrderNumber}",
        };
        var service = new SessionService();
        var session = await service.CreateAsync(stripeSessionOption);

        return new CreatePaymentResponse
        {
            StripeSessionId = session.Id,
            StripeSessionUrl = session.Url
        };
    }
}