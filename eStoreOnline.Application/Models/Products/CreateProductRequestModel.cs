namespace eStoreOnline.Application.Models.Products;

public class CreateProductRequestModel
{
    public int? Id { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public decimal Price { get; set; }
    public string Sku { get; set; }
    public string UrlSlug { get; set; }
    public string ImageUrl { get; set; }
}