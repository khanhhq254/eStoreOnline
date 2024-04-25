using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail(int id)
    {
        return View();
    }
}