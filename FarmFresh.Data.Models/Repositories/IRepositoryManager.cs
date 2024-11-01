namespace FarmFresh.Repositories.Contacts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }
    Task SaveAsync();
}
