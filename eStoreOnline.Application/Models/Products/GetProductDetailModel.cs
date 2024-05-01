namespace eStoreOnline.Application.Models.Products;

public class GetProductDetailModel
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Sku { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string UrlSlug { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}