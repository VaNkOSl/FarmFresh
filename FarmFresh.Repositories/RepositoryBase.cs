using FarmFresh.Data;
using FarmFresh.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly DbContext _data;

    protected RepositoryBase(FarmFreshDbContext data)
    {
        _data = data;
    }

    private DbSet<T> DbSet<T>() where T : class => _data.Set<T>();

    public async Task CreateAsync(T entity) => await _data.AddAsync(entity);

    public void Delete(T entity) => _data.Remove(entity);

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
        _data.Set<T>()
        .AsNoTracking() :
        _data.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
         !trackChanges ?
        _data.Set<T>()
        .Where(expression)
        .AsNoTracking() :
        _data.Set<T>()
        .Where(expression);

    public void Update(T entity) => _data.Update(entity);

    public async Task<T?> GetByIdAsync(Guid id) => await DbSet<T>().FindAsync(id);
}
