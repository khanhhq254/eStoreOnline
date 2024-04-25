using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Products;
using eStoreOnline.Domain.Entities;
using eStoreOnline.Domain.Exceptions;
using eStoreOnline.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eStoreOnline.Application.Implementations;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedModel<GetAllProductModel>> GetAllProductsAsync(GetAllProductRequestModel request)
    {
        var count = await _context.Products.CountAsync(x => string.IsNullOrWhiteSpace(x.DeletedBy));

        var products = await _context.Products
            .Take(request.PageSize)
            .Skip(request.PageSize * request.PageIndex)
            .Where(x => string.IsNullOrWhiteSpace(x.DeletedBy))
            .Select(x => new GetAllProductModel
            {
                Id = x.Id,
                ProductName = x.ProductName,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Price = x.Price,
                ImageUrl = x.ImageUrl,
            }).ToListAsync();

        return PaginatedModel<GetAllProductModel>.Success(products, request.PageIndex, request.PageSize, count);
    }

    public async Task<GetProductDetailModel> GetProductDetailAsync(string urlSlug)
    {
        var product = await _context.Products
            .Where(x => x.UrlSlug == urlSlug)
            .Select(x => new GetProductDetailModel
            {
                Id = x.Id,
                ProductName = x.ProductName,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Price = x.Price,
                ImageUrl = x.ImageUrl,
            }).FirstOrDefaultAsync();
        if (product == null)
            throw new NotFoundException(nameof(Product), urlSlug);

        return product;
    }

    public async Task<int> UpsertProductAsync(CreateProductRequestModel model)
    {
        Product? product;
        if (model.Id.HasValue)
        {
            product = await _context.Products.FindAsync(model.Id.Value);

            if (product == null)
                throw new NotFoundException(nameof(Product), model.Id.Value);

            product.ProductName = model.ProductName;
            product.Description = model.Description;
            product.ShortDescription = model.ShortDescription;
            product.Price = model.Price;
            product.Sku = model.Sku;
            product.UrlSlug = model.UrlSlug;
            product.ImageUrl = model.ImageUrl;
        }
        else
        {
            product = new Product()
            {
                ProductName = model.ProductName,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                Price = model.Price,
                Sku = model.Sku,
                UrlSlug = model.UrlSlug,
                ImageUrl = model.ImageUrl
            };

            await _context.Products.AddAsync(product);
        }

        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        // product.DeletedBy = ;
        product.DeletedDate = DateTime.UtcNow;

        return true;
    }
}