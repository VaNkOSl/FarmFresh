using FarmFresh.Data.Models.Repositories.Econt;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;

namespace FarmFresh.Services.Contacts;

public interface IServiceManager
{
    IFarmerService FarmerService { get; }

    IAdminService AdminService { get; }

    ICategoryService CategoryService { get; }


    //Econt Services

    IEcontNumenclaturesService EcontNumenclaturesService { get; }

    IEcontAddressService EcontAddressService { get; }

    IEcontLabelService EcontLabelService { get; }

    IEcontShipmentService EcontShipmentService { get; }

    //Econt database specific

    ICountryService CountryService { get; }

    ICityService CityService { get; }

    IOfficeService OfficeService { get; }

    IAddressService AddressService { get; }

    IStreetService StreetService { get; }

    IQuarterService QuarterService { get; }
}
