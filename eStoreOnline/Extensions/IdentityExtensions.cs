using System.Security.Claims;

namespace eStoreOnline.Extensions;

public static class IdentityExtensions
{
    public static string? GetCurrentUserId(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}