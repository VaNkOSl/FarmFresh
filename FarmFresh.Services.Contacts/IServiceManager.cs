using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;

namespace FarmFresh.Services.Contacts;

public interface IServiceManager
{
    IFarmerService FarmerService { get; }

    IAdminService AdminService { get; }

    ICategoryService CategoryService { get; }


    //API Services

    IEcontNumenclaturesService EcontNumenclaturesService { get; }

    //Database Services

    ICountryService CountryService { get; }
}
