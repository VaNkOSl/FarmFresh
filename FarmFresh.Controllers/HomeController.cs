using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static FarmFresh.Commons.GeneralApplicationConstants;

namespace FarmFresh.Controllers;

public class HomeController : BaseController
{
    private readonly IMemoryCache _memoryCache;
    private readonly IRepositoryManager _repositoryManager;

    public HomeController(IMemoryCache memoryCache, IRepositoryManager repositoryManager)
    {
        _memoryCache = memoryCache;
        _repositoryManager = repositoryManager;
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole(AdminRoleName))
        {
            return RedirectToAction("DashBoard", "Home", new { area = AdminAreaName });
        }


        var farmersLocations = await GetFarmerLocations();
        ViewData["FarmersLocations"] = farmersLocations;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode)
    {
        return View();
    }

    public async Task<List<FarmerLocation>> GetFarmerLocations()
    {
        var cachedLocations = _memoryCache.Get<List<FarmerLocation>>("allFarmerLocations");
        if (cachedLocations != null)
        {
            return cachedLocations; 
        }

        var locations = await _repositoryManager.FarmerLocationRepository
            .GetAllLocationsAsync(trackChanges: false) 
            .ToListAsync(); 

        _memoryCache.Set("allFarmerLocations", locations, TimeSpan.FromHours(1));

        return locations; 
    }

}
