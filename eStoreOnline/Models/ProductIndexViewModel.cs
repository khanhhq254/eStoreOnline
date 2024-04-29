using eStoreOnline.Application.Models.Products;

namespace eStoreOnline.Models;

public class ProductIndexViewModel
{
    public List<GetAllProductModel> RecentProducts { get; set; } = [];
    public int PageIndex { get; set; }
    public int TotalItem { get; set; }
    public int PageSize { get; set; }

    public List<string> Pages
    {
        get
        {
            var pages = new List<string>();
            if (TotalPage <= 8)
            {
                for (var i = 1; i <= TotalPage; i++)
                {
                    pages.Add(i.ToString());
                }

                return pages;
            }

            for (var i = 1; i <= 3; i++)
            {
                pages.Add(i.ToString());
            }

            pages.Add("...");
            for (var i = TotalPage - 3; i <= TotalPage; i++)
            {
                pages.Add(i.ToString());
            }

            return pages;
        }
    }

    public int TotalPage => (int) Math.Ceiling(TotalItem / (double) PageSize);
}