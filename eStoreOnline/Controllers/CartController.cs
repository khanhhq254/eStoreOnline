using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Carts;
using eStoreOnline.Models;
using eStoreOnline.Application.Models.Orders;
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
    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetCartByUserAsync(GetCurrentUserId());
        return View(cart);
    }

    public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartModel request)
    {
        request.UserId = GetCurrentUserId();
        await _cartService.AddItemToCartAsync(request);

        return Ok(true);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateCartRequestModel request)
    {
        request.UserId = GetCurrentUserId();
        var result = await _cartService.UpdateCartAsync(request);

        return Ok(ResponseModel<CartModel>.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItemFromCart(RemoveItemFromCartModel request)
    {
        request.UserId = GetCurrentUserId();
        await _cartService.RemoveItemFromCartAsync(request);

        return Ok(ResponseModel.Success());
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentCart()
    {
        var cart = await _cartService.GetCartByUserAsync(GetCurrentUserId());

        return Ok(ResponseModel<CartModel>.Success(cart));
    }

    [HttpPost]
    public async Task<JsonResult> CheckOut(int cartId)
    {
        await _orderService.CreateOrderAsync(new CreateOrderRequestModel()
        {
            Domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value,
            CartId = cartId,
            UserId = GetCurrentUserId()
        });

        return Json("OK");
    }
}