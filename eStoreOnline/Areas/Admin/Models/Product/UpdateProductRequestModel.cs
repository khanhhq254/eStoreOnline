using System.ComponentModel.DataAnnotations;

namespace eStoreOnline.Areas.Admin.Models.Product;

public class UpdateProductRequestModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Sku { get; set; } = string.Empty;
    [MaxLength(2048)]
    public string Description { get; set; } = string.Empty;
    [MaxLength(512)]
    public string ShortDescription { get; set; } = string.Empty;
    [Required]
    [Range(0, 1000000)]
    public decimal Price { get; set; }
    [Required]
    [MaxLength(200)]
    public string UrlSlug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
    public bool IsDeleted { get; set; }
}