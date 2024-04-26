using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Carts;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> AddItemToCart([FromBody]AddItemToCartModel request)
    {
        await _cartService.AddItemToCartAsync(request);
        return Json("Ok");
    }
}