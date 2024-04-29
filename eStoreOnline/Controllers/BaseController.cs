using eStoreOnline.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Controllers;

public class BaseController : Controller
{
    public string GetCurrentUserId()
    {
        var currentUser = User.GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(currentUser))
        {
            throw new Exception("User not found");
        }

        return currentUser;
    }
}