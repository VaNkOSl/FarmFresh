namespace FarmFresh.Services.Contacts.Econt
{
    public interface IAddressService
    {
        Task DeleteOrphanedAddressesAsync();
    }
}
