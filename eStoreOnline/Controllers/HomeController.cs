using System.Diagnostics;
using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Products;
using Microsoft.AspNetCore.Mvc;
using eStoreOnline.Models;
using eStoreOnline.Models.Home;

namespace eStoreOnline.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;

    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _productService.GetAllProductsAsync(new GetAllProductRequestModel());
        return View(new HomeIndexViewModel()
        {
            RecentProducts = result.Data
        });
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