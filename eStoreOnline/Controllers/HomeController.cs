using System.Diagnostics;
using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Products;
using Microsoft.AspNetCore.Mvc;
using eStoreOnline.Models;
using eStoreOnline.Models.Home;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace eStoreOnline.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly SignInManager<IdentityUser> _signInManager;

    public HomeController(IProductService productService, SignInManager<IdentityUser> signInManager)
    {
        _productService = productService;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _productService.GetAllTopProductsAsync();
        return View(new HomeIndexViewModel()
        {
            RecentProducts = result
        });
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        
        return Ok();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}