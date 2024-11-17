﻿using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface IFarmerRepository
{
    Task<Farmer> CreateFarmerAsync(Farmer farmer);

    Task<Farmer?> GetFarmerByIdAsync(Guid id);

    void DeleteFarmer(Farmer farmer);

    void UpdateFarmer(Farmer farmer);

    IQueryable<Farmer> FindFarmersByConditionAsync(Expression<Func<Farmer, bool>> expression, bool trackChanges);
}
