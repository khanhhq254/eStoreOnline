namespace eStoreOnline.Application.Models;

public class PaginatedModel<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public List<T> Data { get; set; } = [];
    
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