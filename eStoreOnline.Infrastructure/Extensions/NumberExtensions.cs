namespace eStoreOnline.Infrastructure.Extensions;

public static class NumberExtensions
{
    public static string ToMoney(this decimal value)
    {
        return value.ToString("0.00");
    }
}