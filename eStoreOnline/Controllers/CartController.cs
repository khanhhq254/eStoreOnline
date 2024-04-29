using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Carts;
using eStoreOnline.Application.Models.Orders;
using eStoreOnline.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Controllers;

[Authorize]
public class CartController : BaseController
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;

    public CartController(ICartService cartService, IOrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartModel request)
    {
        await _cartService.AddItemToCartAsync(request);
        return Json("Ok");
    }

    [HttpGet]
    public async Task<JsonResult> GetCurrentCart()
    {
        var result = await _cartService.GetCartByUserAsync(new CartRequestModel()
        {
            UserId = CurrentUserId!
        });

        return Json(result);
    }

    [HttpPost]
    public async Task<JsonResult>
        RemoveItemFromCart(RemoveItemFromCartModel request)
    {
        request.UserId = CurrentUserId!;

        await _cartService.RemoveItemFromCartAsync(request);

        var result = await _cartService.GetCartByUserAsync(new CartRequestModel()
        {
            UserId = CurrentUserId!
        });

        return Json(result);
    }
    
    [HttpPost]
    public async Task<JsonResult> CheckOut(int cartId)
    {
        await _orderService.CreateOrderAsync(new CreateOrderRequestModel()
        {
            Domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value,
            CartId = cartId,
            UserId = CurrentUserId!
        });
        
        return Json("OK");
    }
}