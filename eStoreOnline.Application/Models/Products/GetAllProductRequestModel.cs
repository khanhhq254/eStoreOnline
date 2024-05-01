namespace eStoreOnline.Application.Models.Products;

public class GetAllProductRequestModel : BasePaginatedRequest
{
    public string? SearchText { get; set; }
}