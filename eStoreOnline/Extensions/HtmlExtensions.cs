using eStoreOnline.Domain.Enums;

namespace eStoreOnline.Extensions;

public static class HtmlExtensions
{
    public static string ToCssStyle(this OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "bg-gray",
            OrderStatus.Completed => "bg-primary text-light",
            OrderStatus.Cancelled => "bg-warning",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}