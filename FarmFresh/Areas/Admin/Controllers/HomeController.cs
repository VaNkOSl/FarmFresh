using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Areas.Admin.Controllers;

public class HomeController : AdminBaseController
{
    [HttpGet]
    public IActionResult DashBoard() =>
        View();
}
