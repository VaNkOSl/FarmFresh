using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Areas.Admin.Controllers;

[Route("api/admin/home")]
public class HomeController : AdminBaseController
{
    [HttpGet("dashboard")]
    public IActionResult DashBoard() =>
        View();
}
