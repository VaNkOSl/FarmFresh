using FarmFresh.Data.Models.Econt.Nomenclatures;
using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories.Econt
{
    public interface IAddressRespository
    {
        Task<Address> CreateAddressAsync(Address address);
        void DeleteAddress(Address address);
        void UpdateAddress(Address address);
        IQueryable<Address> FindAllAddresses(bool trackChanges);
        IQueryable<Address> FindAddressesByCondition(Expression<Func<Address, bool>> expression, bool trackChanges);
        Address? FindFirstAddressByCondition(Expression<Func<Address, bool>> expression, bool trackChanges);
        Task<Address?> FindFirstAddressByConditionAsync(Expression<Func<Address, bool>> expression, bool trackChanges);
        Task DeleteOrphanedAddressesAsync();
    }
}
