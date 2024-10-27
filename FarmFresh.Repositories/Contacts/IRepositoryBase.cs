namespace FarmFresh.Repositories.Contacts;

public interface IRepositoryBase<T>
{
    IQueryable<T> All<T>() where T : class;

    IQueryable<T> AllReadOnly<T>() where T : class;

    Task AddAsync<T>(T entity) where T : class;

    Task<T?> GetByIdAsync<T>(Guid id) where T : class;

    Task DeleteAsync<T>(Guid id) where T : class;

    Task UpdateAsync<T>(T entity) where T : class;

    Task DeleteRange<T>(IEnumerable<T> entities) where T : class;
}
