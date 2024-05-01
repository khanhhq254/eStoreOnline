using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models.Products;
using eStoreOnline.Areas.Admin.Models.Product;
using eStoreOnline.Domain.Commons;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Areas.Admin.Controllers;

public class ProductController : AdminBaseController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(GetAllProductRequestModel request)
    {
        if (request.PageIndex != 0)
        {
            request.PageIndex--;
        }

        ViewBag.SearchText = request.SearchText ?? string.Empty;
        var result = await _productService.GetAllProductsAsync(request);

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return await Task.Run(View);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequestModel request)
    {
        if (ModelState.IsValid)
        {
            using var memoryStream = new MemoryStream();
            await request.Image.CopyToAsync(memoryStream);

            try
            {
                var product = new UpsertProductRequestModel()
                {
                    ShortDescription = request.ShortDescription,
                    Description = request.Description,
                    Price = request.Price,
                    UrlSlug = request.UrlSlug,
                    ProductName = request.Name,
                    Sku = request.Sku,
                    Image = memoryStream,
                    ImageName = request.Image?.FileName
                };

                await _productService.UpsertProductAsync(product);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Model", e.Message);
                return View(request);
            }

            return RedirectToAction("Index", "Product", new { area = AreaConstants.AdminAreaName });
        }

        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        ViewBag.Id = id;
        var product = await _productService.GetProductDetailAsync(id);
        var model = new UpdateProductRequestModel()
        {
            Name = product.ProductName,
            Sku = product.Sku,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            UrlSlug = product.UrlSlug,
            ShortDescription = product.ShortDescription,
            IsDeleted = product.IsDeleted
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Detail(int id, [FromForm] UpdateProductRequestModel request)
    {
        ViewBag.Id = id;
        if (ModelState.IsValid)
        {
            using var memoryStream = new MemoryStream();
            if (request.Image != null)
            {
                await request.Image.CopyToAsync(memoryStream);
            }

            try
            {
                var product = new UpsertProductRequestModel()
                {
                    Id = id,
                    ShortDescription = request.ShortDescription,
                    Description = request.Description,
                    Price = request.Price,
                    ImageUrl = request.ImageUrl,
                    UrlSlug = request.UrlSlug,
                    ProductName = request.Name,
                    Sku = request.Sku,
                    Image = memoryStream,
                    ImageName = request.Image?.FileName
                };

                await _productService.UpsertProductAsync(product);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Model", e.Message);
                return View(request);
            }

            return RedirectToAction("Index", "Product", new { area = AreaConstants.AdminAreaName });
        }

        return View(request);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction("Index", "Product", new { area = AreaConstants.AdminAreaName });
    }
}