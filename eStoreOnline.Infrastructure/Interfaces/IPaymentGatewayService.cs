using eStoreOnline.Infrastructure.Models;

namespace eStoreOnline.Infrastructure.Interfaces;

public interface IPaymentGatewayService
{
    Task<CreatePaymentResponse> CreatePaymentSessionAsync(CreatePaymentSessionModel request);
}