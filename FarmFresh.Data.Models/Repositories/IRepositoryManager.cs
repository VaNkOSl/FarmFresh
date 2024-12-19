using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories.Econt;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;

namespace FarmFresh.Data.Models.Repositories;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }

    IFarmerRepository FarmerRepository { get; }

    IFarmerLocationRepository FarmerLocationRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    ICountryRepository CountryRepository { get; }

    ICityRepository CityRepository { get; }

    IOfficeRepository OfficeRepository { get; }

    IAddressRespository AddressRepository { get; }

    IStreetRepository StreetRepository { get; }

    IQuarterRepository QuarterRepository { get; }

    IProductRepository ProductRepository { get; }

    IProductPhotoRepository ProductPhotoRepository { get; }

    IReviewRepository ReviewRepository { get; }

    IOrderRepository OrderRepository { get; }

    IOrderProductRepository OrderProductRepository { get; }

    ICartItemRepository CartItemRepository { get; }

    Task<CRUDResult> SaveAsync(Entity entity);

    Task SaveAsync();
}
