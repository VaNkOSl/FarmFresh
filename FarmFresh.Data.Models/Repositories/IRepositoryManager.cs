using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories.Econt;
using FarmFresh.Repositories.Contacts;

namespace FarmFresh.Data.Models.Repositories;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }

    IFarmerRepository FarmerRepository { get; }

    IFarmerLocationRepository FarmerLocationRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    //Econt database specific

    ICountryRepository CountryRepository { get; }

    ICityRepository CityRepository { get; }

    IOfficeRepository OfficeRepository { get; }

    IAddressRespository AddressRepository { get; }

    IStreetRepository StreetRepository { get; }

    IQuarterRepository QuarterRepository { get; }

    Task SaveAsync(Entity entity);

    Task SaveAsync();
}
