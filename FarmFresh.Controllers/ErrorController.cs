using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Controllers;

[Route("api/error")]
public class ErrorController : BaseController
{
    [HttpGet("unauthorized")]
    public IActionResult UnauthorizedError()
    {
        return View("Unauthorized");
    }

    [HttpGet("notfound")]
    public IActionResult NotFoundError()
    {
        return View("NotFound");
    }

    [HttpGet("internalserverError")]
    public IActionResult InternalServerError() =>
        View("InternalServerError");
    

    [HttpGet("general")]
    public IActionResult GeneralError()
    {
        return View("Error");
    }
}
