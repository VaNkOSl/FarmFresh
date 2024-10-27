using FarmFresh.Data;
using FarmFresh.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T>
{
    private readonly DbContext _data;

    protected RepositoryBase(FarmFreshDbContext data)
    {
        _data = data;
    }

    private DbSet<T> DbSet<T>() where T : class => _data.Set<T>();

    public async Task AddAsync<T>(T entity) where T : class => await DbSet<T>().AddAsync(entity);

    public IQueryable<T> All<T>() where T : class => DbSet<T>();

    public IQueryable<T> AllReadOnly<T>() where T : class => DbSet<T>().AsNoTracking();

    public async Task DeleteAsync<T>(Guid id) where T : class
    {
        T? entity = await GetByIdAsync<T>(id);

        if (entity != null)
        {
            DbSet<T>().Remove(entity);
        }
    }

    public Task DeleteRange<T>(IEnumerable<T> entities) where T : class
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : class => await DbSet<T>().FindAsync(id);

    public async Task UpdateAsync<T>(T entity) where T : class
    {
        _data.Update(entity);
        await this._data.SaveChangesAsync();
    }
}
