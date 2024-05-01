using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Products;
using eStoreOnline.Domain.Commons;
using eStoreOnline.Domain.Entities;
using eStoreOnline.Domain.Exceptions;
using eStoreOnline.Infrastructure.Data;
using eStoreOnline.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eStoreOnline.Application.Implementations;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IStorageService _storageService;
    private readonly IIdentityPrincipal _identityPrincipal;

    public ProductService(ApplicationDbContext context, IStorageService storageService, IIdentityPrincipal identityPrincipal)
    {
        _context = context;
        _storageService = storageService;
        _identityPrincipal = identityPrincipal;
    }

    public async Task<PaginatedModel<GetAllProductModel>> GetAllProductsAsync(GetAllProductRequestModel request)
    {
        var query = _context.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            query = query.Where(n => n.ProductName.ToLower().Contains(request.SearchText.ToLower()));
        }
        
        var count = await query.CountAsync(x => string.IsNullOrWhiteSpace(x.DeletedBy));

        var products = await query
            .OrderByDescending(x => x.CreatedDate)
            .Where(x => string.IsNullOrWhiteSpace(x.DeletedBy))
            .Skip(request.PageSize * request.PageIndex)
            .Take(request.PageSize)
            .Select(x => new GetAllProductModel
            {
                Id = x.Id,
                ProductName = x.ProductName,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Price = x.Price,
                ImageUrl = x.ImageUrl,
                UrlSlug = x.UrlSlug,
                Sku = x.Sku,
            }).ToListAsync();

        return PaginatedModel<GetAllProductModel>.Success(products, request.PageIndex, request.PageSize, count);
    }

    public async Task<List<GetAllProductModel>> GetAllTopProductsAsync()
    {
        var count = await _context.Products.CountAsync(x => string.IsNullOrWhiteSpace(x.DeletedBy));

        var products = await _context.Products
            .Take(10)
            .Skip(0)
            .Where(x => string.IsNullOrWhiteSpace(x.DeletedBy))
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new GetAllProductModel
            {
                Id = x.Id,
                ProductName = x.ProductName,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Price = x.Price,
                ImageUrl = x.ImageUrl,
            }).ToListAsync();

        return products;
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
                Sku = x.Sku,
                UrlSlug = x.UrlSlug,
                IsDeleted = !string.IsNullOrWhiteSpace(x.DeletedBy)
            }).FirstOrDefaultAsync();
        if (product == null)
            throw new NotFoundException(nameof(Product), urlSlug);

        return product;
    }
    
    public async Task<GetProductDetailModel> GetProductDetailAsync(int productId)
    {
        var product = await _context.Products
            .Where(x => x.Id == productId)
            .Select(x => new GetProductDetailModel
            {
                Id = x.Id,
                ProductName = x.ProductName,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Price = x.Price,
                ImageUrl = x.ImageUrl,
                Sku = x.Sku,
                UrlSlug = x.UrlSlug,
                IsDeleted = !string.IsNullOrWhiteSpace(x.DeletedBy)
            }).FirstOrDefaultAsync();
        if (product == null)
            throw new NotFoundException(nameof(Product), productId);

        return product;
    }

    public async Task<int> UpsertProductAsync(UpsertProductRequestModel model)
    {
        Product? product;
        if (string.IsNullOrWhiteSpace(model.ImageUrl) && model.Image == null)
        {
            throw new ArgumentException("Image is required");
        }

        if (model.Image != null && !string.IsNullOrWhiteSpace(model.ImageName))
        {
            var path = await _storageService.UploadFileAsync(model.Image, model.ImageName,
                ProductConstants.ProductImageContainerName);
            model.ImageUrl = path;
        }
        
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
            product.ModifiedBy = _identityPrincipal.GetCurrentUserId();
            product.ModifiedDate = DateTime.UtcNow;
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
                ImageUrl = model.ImageUrl,
                CreatedBy = _identityPrincipal.GetCurrentUserId(),
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = _identityPrincipal.GetCurrentUserId(),
                ModifiedDate = DateTime.UtcNow
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

        product.DeletedDate = DateTime.UtcNow;
        product.DeletedBy = _identityPrincipal.GetCurrentUserId();
        
        await _context.SaveChangesAsync();

        return true;
    }
}