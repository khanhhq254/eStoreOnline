namespace eStoreOnline.Application.Models;

public class PaginatedModel<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int) Math.Ceiling(TotalCount / (double) PageSize);
    public List<T> Data { get; set; } = [];

    public List<string> Pages
    {
        get
        {
            var pages = new List<string>();
            if (TotalPages <= 8)
            {
                for (var i = 1; i <= TotalPages; i++)
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
            for (var i = TotalPages - 3; i <= TotalPages; i++)
            {
                pages.Add(i.ToString());
            }

            return pages;
        }
    }

    public static PaginatedModel<T> Success(List<T> data, int pageIndex, int pageSize, int totalCount)
    {
        return new PaginatedModel<T>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalCount,
            Data = data
        };
    }
}