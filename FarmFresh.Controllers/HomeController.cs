using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Controllers;

public class HomeController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode)
    {
        return View();
    }
}
