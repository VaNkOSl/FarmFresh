using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using FarmFresh.Services.Contacts.FarmersInterfaces;
using FarmFresh.Services.Contacts.ProductsInterfaces;

namespace FarmFresh.Services.Contacts;

public interface IServiceManager
{
    IFarmerService FarmerService { get; }

    IAdminService AdminService { get; }

    ICategoryService CategoryService { get; }

    IEcontNumenclaturesService EcontNumenclaturesService { get; }

    IEcontAddressService EcontAddressService { get; }

    IEcontLabelService EcontLabelService { get; }

    IEcontShipmentService EcontShipmentService { get; }

    ICountryService CountryService { get; }

    ICityService CityService { get; }

    IOfficeService OfficeService { get; }

    IAddressService AddressService { get; }

    IStreetService StreetService { get; }

    IQuarterService QuarterService { get; }

    IProductService ProductService { get; }

    IProductPhotoService ProductPhotoService { get; }

    IReviewService ReviewService { get; }

    IOrderService OrderService { get; }

    ICartService CartService { get; }

    #region Service Extensions
    IProductManagmentService ProductManagmentService { get; }

    IProductsQueryService ProductsQueryService { get; }

    IFarmerQueryService FarmerQueryService { get; }

    IFarmerValidationService FarmerValidationService { get; }

    IFarmerManagementService FarmerManagementService { get; }
    #endregion
}
