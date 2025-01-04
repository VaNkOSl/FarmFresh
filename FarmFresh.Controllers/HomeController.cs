using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Infrastructure.Extensions;
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
        if (User.IsAdmin())
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
        if (statusCode == 400 || statusCode == 404)
        {
            return View("Error404");
        }

        if (statusCode == 401)
        {
            return View("Error401");
        }

        return View();
    }

    private async Task<List<FarmerLocation>> GetFarmerLocations()
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
