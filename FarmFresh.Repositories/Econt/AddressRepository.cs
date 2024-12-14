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
    public class AddressRepository(FarmFreshDbContext data) : RepositoryBase<Address>(data), IAddressRespository
    {
        public async Task<Address> CreateAddressAsync(Address address)
        {
            await CreateAsync(address);
            return address;
        }

        public void DeleteAddress(Address address)
            => Delete(address);

        public async Task DeleteOrphanedAddressesAsync()
        {
            //Where clause to be updated when a new entity is made
            //that contains Address as a field (ShippingLabel in the future)

            var orphanedAddresses = await _data.Addresses
                .Where(a => !_data.Offices.Any(o => o.AddressId == a.Id))
                .ToListAsync();

            _data.Addresses.RemoveRange(orphanedAddresses);
            await _data.SaveChangesAsync();
        }

        public IQueryable<Address> FindAddressesByCondition(Expression<Func<Address, bool>> expression, bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public IQueryable<Address> FindAllAddresses(bool trackChanges)
            => FindAll(trackChanges);

        public Address? FindFirstAddressByCondition(Expression<Func<Address, bool>> expression, bool trackChanges)
            => FindFirstByCondition(expression, trackChanges);

        public async Task<Address?> FindFirstAddressByConditionAsync(Expression<Func<Address, bool>> expression, bool trackChanges)
            => await FindFirstByConditionAsync(expression, trackChanges);

        public async Task<Address?> GetAddressByIdAsync(int id)
            => await GetByIdAsync(id);

        public void UpdateAddress(Address address)
            => Update(address);
    }
}
