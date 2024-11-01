using FarmFresh.Data;
using FarmFresh.Repositories.Contacts;

namespace FarmFresh.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly FarmFreshDbContext _data;
    private Lazy<IUserRepository> _userRepository;

    public RepositoryManager(FarmFreshDbContext data)
    {
        _data = data;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(data));
    }

    public IUserRepository UserRepository => _userRepository.Value;

    public async Task SaveAsync() => await _data.SaveChangesAsync();    
}
