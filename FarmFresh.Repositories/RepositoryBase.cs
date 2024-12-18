﻿using FarmFresh.Data;
using FarmFresh.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;
using NLog.Targets.Wrappers;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly FarmFreshDbContext _data;

    protected RepositoryBase(FarmFreshDbContext data)
    {
        _data = data;
    }

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

    public T? FindFirstByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
        _data.Set<T>()
            .AsNoTracking()
            .FirstOrDefault(expression) :
        _data.Set<T>()
            .FirstOrDefault(expression);

    public Task<T?> FindFirstByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
        _data.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(expression) :
        _data.Set<T>()
            .FirstOrDefaultAsync(expression);

    public void Update(T entity) => _data.Update(entity);
}
