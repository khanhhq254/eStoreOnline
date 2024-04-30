using eStoreOnline.Application.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using StripeConfiguration = eStoreOnline.Infrastructure.Configurations.StripeConfiguration;

namespace eStoreOnline.Controllers;

[AllowAnonymous]
public class WebhookController() : Controller
{
    [HttpPost]
    [Route("webhook")]
    public async Task<ActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var request = new OrderProcessingRequestModel
        {
            Signature = Request.Headers["Stripe-Signature"],
            BodyContent = json
        };
       
        return Ok(json);
    }
}