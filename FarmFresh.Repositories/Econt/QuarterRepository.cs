using FarmFresh.Data;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Repositories.Econt
{
    public class QuarterRepository(FarmFreshDbContext data) : RepositoryBase<Quarter>(data), IQuarterRepository
    {
        public async Task<Quarter> CreateQuarterAsync(Quarter quarter)
        {
            await CreateAsync(quarter);
            return quarter;
        }

        public void DeleteQuarter(Quarter quarter)
            => Delete(quarter);

        public IQueryable<Quarter> FindAllQuarters(bool trackChanges)
            => FindAll(trackChanges);

        public Quarter? FindFirstQuarterByCondition(Expression<Func<Quarter, bool>> expression, bool trackChanges)
            => FindFirstByCondition(expression, trackChanges);

        public async Task<Quarter?> FindFirstQuarterByConditionAsync(Expression<Func<Quarter, bool>> expression, bool trackChanges)
            => await FindFirstByConditionAsync(expression, trackChanges);

        public IQueryable<Quarter> FindQuartersByCondition(Expression<Func<Quarter, bool>> expression, bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public Task<Quarter?> GetQuarterByIdAsync(int id)
            => GetByIdAsync(id);

        public void UpdateQuarter(Quarter quarter)
            => Update(quarter);

        public async Task UpdateQuartersAsync(IEnumerable<Quarter> quarters)
        {
            const int batchSize = 1000;

            var quarterBatches = quarters
                .Chunk(batchSize)
                .ToList();

            var citiesIds = await _data.Cities
                .Select(c => c.Id)
                .ToListAsync();
            var citiesIdsHashSet = citiesIds.ToHashSet();

            foreach(var batch in quarterBatches)
            {
                var correctedBatch = batch
                    .Where(q => citiesIdsHashSet.Contains((int)q.CityID!))
                    .ToList();

                var quarterIds = correctedBatch
                    .Select(q => q.Id)
                    .ToList();
                var quarterIdsHashSet = quarterIds.ToHashSet();

                var existingQuarters = await _data.Quarters
                    .Where(q => quarterIdsHashSet.Contains(q.Id))
                    .ToDictionaryAsync(q => q.Id);

                var quartersToAdd = new List<Quarter>();

                foreach(var quarter in correctedBatch)
                {
                    if(existingQuarters.TryGetValue(quarter.Id, out var existingQuarter))
                    {
                        existingQuarter.CityID = quarter.CityID;
                        existingQuarter.Name = quarter.Name;
                        existingQuarter.NameEn = quarter.NameEn;

                        existingQuarters.Remove(quarter.Id);
                    }
                    else if(!quartersToAdd.Contains(quarter))
                        quartersToAdd.Add(quarter);
                }

                if(quartersToAdd.Count > 0)
                    await _data.Quarters.AddRangeAsync(quartersToAdd);

                if(existingQuarters.Count > 0)
                    _data.Quarters.RemoveRange(existingQuarters.Values);
            }

            await _data.SaveChangesAsync();
        }
    }
}
