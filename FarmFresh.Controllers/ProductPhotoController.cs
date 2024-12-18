using FarmFresh.Services.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Controllers;

[Route("api/productphoto")]
public class ProductPhotoController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public ProductPhotoController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpDelete("delete/{photoId}")]
    public async Task<IActionResult> DeleteImage(Guid photoId)
    {
        try
        {
            await _serviceManager.ProductPhotoService.DeleteProductPhotoAsync(photoId, trackChanges: true);
            return Ok(new { success = true, message = "Photo deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
