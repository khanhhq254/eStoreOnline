using eStoreOnline.Application.Models;
using eStoreOnline.Application.Models.Products;

namespace eStoreOnline.Application.Interfaces;

public interface IProductService
{
    Task<PaginatedModel<GetAllProductModel>> GetAllProductsAsync(GetAllProductRequestModel request);
    Task<List<GetAllProductModel>> GetAllTopProductsAsync();
    Task<GetProductDetailModel> GetProductDetailAsync(string urlSlug);
    Task<GetProductDetailModel> GetProductDetailAsync(int productId);
    Task<int> UpsertProductAsync(UpsertProductRequestModel model);
    Task<bool> DeleteProductAsync(int id);
}