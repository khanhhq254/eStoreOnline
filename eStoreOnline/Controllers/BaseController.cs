using eStoreOnline.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Controllers;

public class BaseController : Controller
{
    protected readonly string? CurrentUserId;

    public BaseController()
    {
        CurrentUserId = User.GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(CurrentUserId))
        {
            throw new Exception("User not found");
        }
    }
}