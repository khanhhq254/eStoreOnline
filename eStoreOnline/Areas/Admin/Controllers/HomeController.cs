using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Areas.Admin.Controllers;

public class HomeController : AdminBaseController
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}