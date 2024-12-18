using FarmFresh.Commons.RequestFeatures;
using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface IFarmerRepository
{
    Task<Farmer> CreateFarmerAsync(Farmer farmer);

    void DeleteFarmer(Farmer farmer);

    void UpdateFarmer(Farmer farmer);

    IQueryable<Farmer> FindFarmersByConditionAsync(Expression<Func<Farmer, bool>> expression, bool trackChanges);

    IQueryable<Farmer> FindAllFarmers(bool trackChanges); 

    Task<PagedList<Farmer>> GetFarmersAsync(FarmerParameters farmerParameters, bool trackChanges);

    Task<PagedList<Farmer>> GetUnapprovedFarmersAsync(FarmerParameters farmerParameters, bool trackChanges);
}
