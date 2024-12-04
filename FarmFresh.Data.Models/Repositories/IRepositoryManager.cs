using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Data.Models.Repositories.Econt;

namespace FarmFresh.Repositories.Contacts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }

    IFarmerRepository FarmerRepository { get; }

    IFarmerLocationRepository FarmerLocationRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    ICountryRepository CountryRepository { get; }

    ICityRepository CityRepository { get; }

    Task SaveAsync(Entity entity);

    Task SaveAsync();
}
