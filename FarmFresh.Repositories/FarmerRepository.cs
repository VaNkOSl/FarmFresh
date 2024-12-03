using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using FarmFresh.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
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

    public IQueryable<Farmer> FindAllFarmers(bool trackChanges) =>
        FindAll(trackChanges);

    public IQueryable<Farmer> FindFarmersByConditionAsync(Expression<Func<Farmer, bool>> expression, bool trackChanges) =>
             FindByCondition(expression, trackChanges);

    public async Task<PagedList<Farmer>> GetFarmersAsync(FarmerParameters farmerParameters, bool trackChanges)
    {
        var farmers = await
            FindAllFarmers(trackChanges)
            .Include(f => f.User)
            .Search(farmerParameters.SearchTerm)
            .ToListAsync();

        return PagedList<Farmer>
            .ToPagedList(farmers, farmerParameters.PageNumber, farmerParameters.PageSize);
    }

    public void UpdateFarmer(Farmer farmer) => Update(farmer);
}