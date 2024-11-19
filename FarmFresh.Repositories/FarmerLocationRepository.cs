using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;

namespace FarmFresh.Repositories;

internal sealed class FarmerLocationRepository(FarmFreshDbContext data, IValidateEntity validateEntity)
    : RepositoryBase<FarmerLocation>(data), IFarmerLocationRepository
{
    public async Task<FarmerLocation> CreateLocationAsync(FarmerLocation location)
    {
        await CreateAsync(location);
        return location;
    }

    public void DeleteLocation(FarmerLocation location) => Delete(location);

    public void UpdateLocation(FarmerLocation location) => Update(location);

    public IQueryable<FarmerLocation> GetAllLocationsAsync(bool trackChanges) =>
        FindAll(trackChanges);
}
