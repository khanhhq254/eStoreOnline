using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Products;

namespace eStoreOnline.Application.Interfaces;

public interface IProductService
{
    Task<PaginatedModel<GetAllProductModel>> GetAllProductsAsync(GetAllProductRequestModel request);
    Task<GetProductDetailModel> GetProductDetailAsync(string urlSlug);
    Task<int> UpsertProductAsync(CreateProductRequestModel model);
    Task<bool> DeleteProductAsync(int id);
}