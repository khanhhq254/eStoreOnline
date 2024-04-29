using System.Security.Claims;
using eStoreOnline.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace eStoreOnline.Infrastructure.Implementations;

public class IdentityPrincipal(IHttpContextAccessor httpContextAccessor) : IIdentityPrincipal
{
    public string GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated ?? false
            ? httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty
            : string.Empty;
    }
}