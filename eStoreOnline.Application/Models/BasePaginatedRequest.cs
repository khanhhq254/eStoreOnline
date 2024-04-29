namespace eStoreOnline.Application.Models;

public class BasePaginatedRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 12;
}