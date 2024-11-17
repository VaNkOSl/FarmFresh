using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal sealed class FarmerRepository(FarmFreshDbContext data, IValidateEntity validateEntity) :
    RepositoryBase<Farmer>(data), IFarmerRepository
{
    public async Task<Farmer> CreateFarmerAsync(Farmer farmer)
    {
        await CreateAsync(farmer);
        return farmer;
    }

    public void DeleteFarmer(Farmer farmer) => Delete(farmer);

    public IQueryable<Farmer> FindFarmersByConditionAsync(Expression<Func<Farmer, bool>> expression, bool trackChanges) =>
             FindByCondition(expression, trackChanges);


    public async Task<Farmer?> GetFarmerByIdAsync(Guid id) => await GetByIdAsync(id);

    public void UpdateFarmer(Farmer farmer) => Update(farmer);
}
