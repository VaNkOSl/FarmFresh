using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface IFarmerLocationRepository
{
    Task<FarmerLocation> CreateLocationAsync(FarmerLocation location);

    void DeleteLocation(FarmerLocation location);

    void UpdateLocation(FarmerLocation location);

    IQueryable<FarmerLocation> GetAllLocationsAsync(bool trackChanges);

    IQueryable<FarmerLocation> FindFarmerLocationsByConditionAsync(Expression<Func<FarmerLocation, bool>> condition, bool trackChanges);
}
