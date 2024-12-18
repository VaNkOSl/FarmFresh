using FarmFresh.Data;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FarmFresh.Repositories.Econt
{
    public class StreetRepository(FarmFreshDbContext data) : RepositoryBase<Street>(data), IStreetRepository
    {
        public async Task<Street> CreateStreetAsync(Street street)
        {
            await CreateAsync(street);
            return street;
        }

        public void DeleteStreet(Street street)
            => Delete(street);

        public IQueryable<Street> FindAllStreets(bool trackChanges)
            => FindAll(trackChanges);

        public Street? FindFirstStreetByCondition(Expression<Func<Street, bool>> expression, bool trackChanges)
            => FindFirstByCondition(expression, trackChanges);

        public async Task<Street?> FindFirstStreetByConditionAsync(Expression<Func<Street, bool>> expression, bool trackChanges)
            => await FindFirstByConditionAsync(expression, trackChanges);

        public IQueryable<Street> FindStreetsByCondition(Expression<Func<Street, bool>> expression, bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public async Task<Street?> GetStreetByIdAsync(int id)
            => await GetByIdAsync(id);

        public void UpdateStreet(Street street)
            => Update(street);

        public async Task UpdateStreetsAsync(IEnumerable<Street> streets)
        {
            const int batchSize = 1000;

            var streetBatches = streets
                .Chunk(batchSize)
                .ToList();

            var citiesIds = await _data.Cities
                .Select(c => c.Id)
                .ToListAsync();
            var citiesIdsHashSet = citiesIds.ToHashSet();

            foreach(var batch in streetBatches)
            {
                var correctedBatch = batch
                    .Where(s => citiesIdsHashSet.Contains((int)s.CityID!))
                    .ToList();

                var streetIds = correctedBatch
                    .Select(s => s.Id)
                    .ToList();
                var streetIdsHashSet = streetIds.ToHashSet();

                var existingStreets = await _data.Streets
                    .Where(s => streetIdsHashSet.Contains(s.Id))
                    .ToDictionaryAsync(s => s.Id);

                var streetsToAdd = new List<Street>();

                foreach(var street in correctedBatch)
                {
                    if(existingStreets.TryGetValue(street.Id, out var existingStreet))
                    {
                        existingStreet.CityID = street.CityID;
                        existingStreet.Name = street.Name;
                        existingStreet.NameEn = street.NameEn;

                        existingStreets.Remove(street.Id);
                    }
                    else if(!streetsToAdd.Contains(street))
                        streetsToAdd.Add(street);
                }

                if(streetsToAdd.Count > 0)
                    await _data.Streets.AddRangeAsync(streetsToAdd);

                if(existingStreets.Count > 0)
                    _data.Streets.RemoveRange(existingStreets.Values);
            }

            await _data.SaveChangesAsync();
        }
    }
}
