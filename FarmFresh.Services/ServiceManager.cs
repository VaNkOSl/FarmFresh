using AutoMapper;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using FarmFresh.Services.Econt;
using FarmFresh.Services.Econt.APIServices;
using LoggerService.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FarmFresh.Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IFarmerService> _farmerService;
    private readonly Lazy<IAdminService> _adminService;
    private readonly Lazy<ICategoryService> _categoryService;
    private readonly Lazy<IProductService> _productService;
    private readonly Lazy<IProductPhotoService> _productPhotoService;
    private readonly Lazy<IReviewService> _reviewService;
    private readonly Lazy<ICartService> _cartService;
    private readonly Lazy<IOrderService> _orderService;

    private readonly Lazy<IEcontNumenclaturesService> _econtNumenclaturesService;

    private readonly Lazy<IEcontAddressService> _econtAddressService;

    private readonly Lazy<IEcontLabelService> _econtLabelService;

    private readonly Lazy<IEcontShipmentService> _econtShipmentService;

    private readonly Lazy<ICountryService> _countryService;

    private readonly Lazy<ICityService> _cityService;

    private readonly Lazy<IOfficeService> _officeService;

    private readonly Lazy<IAddressService> _addressService;

    private readonly Lazy<IStreetService> _streetService;

    private readonly Lazy<IQuarterService> _quarterService;

    public ServiceManager(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILoggerManager loggerManager,
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _farmerService = new Lazy<IFarmerService>(() => new FarmerService(repositoryManager, loggerManager, mapper));
        _adminService = new Lazy<IAdminService>(() => new AdminService(repositoryManager, loggerManager, mapper, configuration));
        _categoryService = new Lazy<ICategoryService>(() => new CategoryService(repositoryManager, loggerManager, mapper));
        _econtNumenclaturesService = new Lazy<IEcontNumenclaturesService>(() => new EcontNumenclaturesService(configuration, httpClient));
        _econtAddressService = new Lazy<IEcontAddressService>(() => new EcontAddressService(configuration, httpClient));
        _econtLabelService = new Lazy<IEcontLabelService>(() => new EcontLabelService(configuration, httpClient));
        _econtShipmentService = new Lazy<IEcontShipmentService>(() => new EcontShipmentService(configuration, httpClient));
        _countryService = new Lazy<ICountryService>(() => new CountryService(EcontNumenclaturesService, repositoryManager, mapper));
        _cityService = new Lazy<ICityService>(() => new CityService(EcontNumenclaturesService, repositoryManager, mapper));
        _officeService = new Lazy<IOfficeService>(() => new OfficeService(EcontNumenclaturesService, repositoryManager, mapper));
        _addressService = new Lazy<IAddressService>(() => new AddressService(repositoryManager, mapper));
        _streetService = new Lazy<IStreetService>(() => new StreetService(EcontNumenclaturesService, repositoryManager, mapper));
        _quarterService = new Lazy<IQuarterService>(() => new QuarterService(EcontNumenclaturesService, repositoryManager, mapper));
        _productService = new Lazy<IProductService>(() => new ProductService(repositoryManager, loggerManager, mapper));
        _productPhotoService = new Lazy<IProductPhotoService>(() => new ProductPhotoService(repositoryManager, loggerManager, mapper));
        _reviewService = new Lazy<IReviewService>(() => new ReviewService(repositoryManager, loggerManager, mapper));
        _cartService = new Lazy<ICartService>(() => new CartService(repositoryManager, loggerManager, mapper));
        _orderService = new Lazy<IOrderService>(() => new OrderService(repositoryManager, loggerManager, mapper));
    }

    public IFarmerService FarmerService => _farmerService.Value;

    public IAdminService AdminService => _adminService.Value;

    public ICategoryService CategoryService => _categoryService.Value;

    public IEcontNumenclaturesService EcontNumenclaturesService => _econtNumenclaturesService.Value;

    public IEcontAddressService EcontAddressService => _econtAddressService.Value;

    public IEcontLabelService EcontLabelService => _econtLabelService.Value;

    public IEcontShipmentService EcontShipmentService => _econtShipmentService.Value;

    public ICountryService CountryService => _countryService.Value;

    public ICityService CityService => _cityService.Value;

    public IOfficeService OfficeService => _officeService.Value;

    public IAddressService AddressService => _addressService.Value;

    public IStreetService StreetService => _streetService.Value;
    
    public IQuarterService QuarterService => _quarterService.Value;

    public IProductService ProductService => _productService.Value;

    public IProductPhotoService ProductPhotoService => _productPhotoService.Value;

    public IReviewService ReviewService => _reviewService.Value;

    public ICartService CartService => _cartService.Value;

    public IOrderService OrderService => _orderService.Value;
}
