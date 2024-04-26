using eStoreOnline.Application.Models.Products;

namespace eStoreOnline.Models.Home;

public class HomeIndexViewModel
{
    public List<GetAllProductModel> RecentProducts { get; set; } = [];
}