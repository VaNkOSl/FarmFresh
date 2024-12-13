using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using FarmFresh.Services.Econt;

namespace FarmFresh.Services.Contacts;

public interface IServiceManager
{
    IFarmerService FarmerService { get; }

    IAdminService AdminService { get; }

    ICategoryService CategoryService { get; }


    //Econt Services

    IEcontNumenclaturesService EcontNumenclaturesService { get; }

    //Econt database specific

    ICountryService CountryService { get; }

    ICityService CityService { get; }

    IOfficeService OfficeService { get; }

    IAddressService AddressService { get; }
}
