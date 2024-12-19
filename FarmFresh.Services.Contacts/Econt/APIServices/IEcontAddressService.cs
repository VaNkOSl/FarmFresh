using FarmFresh.Data.Models.Econt.APIInterraction;

namespace FarmFresh.Services.Contacts.Econt.APIServices
{
    public interface IEcontAddressService
    {
        Task<bool> ValidateAddressAsync(ValidateAddressRequest request);
    }
}
