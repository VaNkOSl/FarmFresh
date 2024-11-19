using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;

namespace FarmFresh.Repositories.Contacts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }

    IFarmerRepository FarmerRepository { get; }

    IFarmerLocationRepository FarmerLocationRepository { get; }

    Task SaveAsync(Entity entity);

    Task SaveAsync();
}
