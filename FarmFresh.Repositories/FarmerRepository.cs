using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories;

internal sealed class FarmerRepository(FarmFreshDbContext data, IValidateEntity validateEntity) :
    RepositoryBase<Farmer>(data), IFarmerRepository
{
    public async Task<Farmer> CreateFarmerAsync(Farmer farmer)
    {
        await AddAsync(farmer);
        await data.SaveChangesAsync();
        return farmer;
    }

    public async Task DeleteFarmerAsync(Guid id)
    {
        await DeleteAsync<Farmer>(id);
        await data.SaveChangesAsync();
    }

    public async Task<IQueryable<Farmer>> GetAllFarmerReadOnlyAsync() => AllReadOnly<Farmer>().AsNoTracking();

    public Task<IQueryable<Farmer>> GetAllFarmersAsync() => Task.FromResult(All<Farmer>());

    public async Task<Farmer> GetFarmerByIdAsync(Guid id) => await GetByIdAsync<Farmer>(id);

    public async Task UpdateFarmerAsync(Farmer farmer)
    {
        await UpdateAsync(farmer);
        await data.SaveChangesAsync();
    }
}
