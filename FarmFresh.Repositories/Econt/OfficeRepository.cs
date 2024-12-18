using FarmFresh.Data;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace FarmFresh.Repositories.Econt
{
    public sealed class OfficeRepository(FarmFreshDbContext data, IAddressRespository addressRespository) : RepositoryBase<Office>(data), IOfficeRepository
    {
        private readonly IAddressRespository _addressRepository = addressRespository;

        public async Task<Office> CreateOfficeAsync(Office office)
        {
            await CreateAsync(office);
            return office;
        }

        public void DeleteOffice(Office office) => Delete(office);

        public IQueryable<Office> FindAllOffices(bool trackChanges) => FindAll(trackChanges);

        public Office? FindFirstOfficeByCondition(Expression<Func<Office, bool>> expression, bool trackChanges)
            => FindFirstByCondition(expression, trackChanges);

        public async Task<Office?> FindFirstOfficeByConditionAsync(Expression<Func<Office, bool>> expression, bool trackChanges)
            => await FindFirstByConditionAsync(expression, trackChanges);

        public IQueryable<Office> FindOfficesByCondition(Expression<Func<Office, bool>> expression, bool trackChanges)
             => FindByCondition(expression, trackChanges)
                .Include(o => o.Address!.City)
                    .ThenInclude(c => c!.Country);

        public void UpdateOffice(Office office) => Update(office);

        public async Task UpdateOfficesAsync(IEnumerable<Office> offices)
        {
            if (offices.IsNullOrEmpty()) return;

            var addressesToAdd = new List<Address>();
            var officesToAdd = new List<Office>();
            var mapAddressesToOffices = new Dictionary<Address, Office>();

            foreach (var office in offices)
            {
                if (office.Address == null || office.Address.City == null) continue;

                var existingCity = await _data.Cities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.Id == office.Address.CityId
                        && c.NameEn == office.Address.City.NameEn);

                if (existingCity == null) continue;

                office.Address.CityId = existingCity.Id;

                var address = await _addressRepository.FindFirstAddressByConditionAsync(
                    a => a.FullAddress == office.Address.FullAddress
                    && a.CityId == office.Address.CityId,
                    false);

                if(address != null)
                    office.AddressId = address.Id;
                else if (!addressesToAdd.Contains(office.Address))
                {
                    office.Address.City = null;

                    addressesToAdd.Add(office.Address);
                    mapAddressesToOffices[office.Address] = office;
                }

                office.Address = null;

                var existingOffice = await FindFirstOfficeByConditionAsync(o => o.Id == office.Id, true);

                if(existingOffice != null)
                    _data.Entry(existingOffice).CurrentValues.SetValues(office);
                else if(!officesToAdd.Contains(office))
                    officesToAdd.Add(office);
            }

            await _data.SaveChangesAsync();

            if(addressesToAdd.Count > 0)
            {
                _data.Addresses.AddRange(addressesToAdd);
                await _data.SaveChangesAsync();

                foreach (var address in addressesToAdd)
                {
                    var updatedAddress = await _addressRepository.FindFirstAddressByConditionAsync(
                        a => a.FullAddress == address.FullAddress
                        && a.CityId == address.CityId
                        && a.Zip == address.Zip,
                        false);

                    if (mapAddressesToOffices.TryGetValue(address, out var relatedOffice) && updatedAddress != null)
                        relatedOffice.AddressId = updatedAddress.Id;
                }
            }

            if(officesToAdd.Count > 0)
                _data.Offices.AddRange(officesToAdd);

            await _data.SaveChangesAsync();
        }
    }
}
