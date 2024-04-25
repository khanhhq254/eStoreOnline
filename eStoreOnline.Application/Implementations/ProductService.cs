using eStoreOnline.Application.Interfaces;
using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Products;

namespace eStoreOnline.Application.Implementations;

public class ProductService : IProductService
{
    public Task<PaginatedModel<GetAllProductModel>> GetAllProductsAsync(GetAllProductRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task<GetProductDetailModel> GetProductDetailAsync(string urlSlug)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpsertProductAsync(CreateProductRequestModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }
}