using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmFresh.Data;
namespace FarmFresh.Controllers;

public class AdminController : Controller
{
    private readonly FarmFreshDbContext _context;

    public AdminController(FarmFreshDbContext context)
    {
        _context = context;
    }

    // GET: Pending Farmers
    public async Task<IActionResult> PendingFarmers()
    {
        var farmers = await _context.Farmers
                                    .Where(f => !f.IsApproved)
                                    .ToListAsync();
        return View(farmers);
    }

    // POST: Approve Farmer
    [HttpPost]
    public async Task<IActionResult> ApproveFarmer(Guid id)
    {
        var farmer = await _context.Farmers.FindAsync(id);
        if (farmer == null)
        {
            return NotFound();
        }

        farmer.IsApproved = true;
        _context.Farmers.Update(farmer);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(PendingFarmers));
    }

    [HttpPost]
    public async Task<IActionResult> RejectFarmer(Guid id)
    {
        var farmer = await _context.Farmers.FindAsync(id);
        if (farmer == null)
        {
            return NotFound();
        }

        _context.Farmers.Remove(farmer);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(PendingFarmers));
    }
}
