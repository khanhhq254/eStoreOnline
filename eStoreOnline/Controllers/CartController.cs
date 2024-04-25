using Microsoft.AspNetCore.Mvc;

namespace eStoreOnline.Controllers;

public class CartController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}