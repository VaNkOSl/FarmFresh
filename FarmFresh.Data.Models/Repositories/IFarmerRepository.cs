namespace FarmFresh.Data.Models.Repositories;

public interface IFarmerRepository
{
    Task<IQueryable<Farmer>> GetAllFarmersAsync();

    Task<IQueryable<Farmer>> GetAllFarmerReadOnlyAsync();

    Task<Farmer> CreateFarmerAsync(Farmer farmer);

    Task<Farmer> GetFarmerByIdAsync(Guid id);

    Task UpdateFarmerAsync(Farmer farmer);

    Task DeleteFarmerAsync(Guid id);
}
