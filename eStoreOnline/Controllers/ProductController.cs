using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Products;
using eStoreOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // GET
    public async Task<IActionResult> Index(int pageIndex = 1)
    {
        var result = await _productService.GetAllProductsAsync(new GetAllProductRequestModel()
        {
            PageIndex = pageIndex - 1
        });
        return View(new ProductIndexViewModel()
        {
            RecentProducts = result.Data,
            PageIndex = result.PageIndex + 1,
            PageSize = result.PageSize,
            TotalItem = result.TotalCount
        });
    }

    public async Task<IActionResult> Detail(string urlSlug)
    {
        var result = await _productService.GetProductDetailAsync(urlSlug);
        return View(result);
    }
}