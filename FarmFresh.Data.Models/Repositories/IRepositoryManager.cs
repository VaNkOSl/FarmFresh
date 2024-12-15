using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;

namespace FarmFresh.Repositories.Contacts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }

    IFarmerRepository FarmerRepository { get; }

    IFarmerLocationRepository FarmerLocationRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    IProductRepository ProductRepository { get; }

    IProductPhotoRepository ProductPhotoRepository { get; }

    IReviewRepository ReviewRepository { get; }

    Task<CRUDResult> SaveAsync(Entity entity);

    Task SaveAsync();
}
