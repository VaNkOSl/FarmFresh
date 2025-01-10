﻿using AutoMapper;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using FarmFresh.Services.Contacts.FarmersInterfaces;
using FarmFresh.Services.Contacts.OrdersInterfaces;
using FarmFresh.Services.Contacts.ProductsInterfaces;
using FarmFresh.Services.Econt;
using FarmFresh.Services.Econt.APIServices;
using FarmFresh.Services.Farmers;
using FarmFresh.Services.Orders;
using FarmFresh.Services.Products;
using LoggerService.Contacts;
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

    #region Service Extensions
    private readonly Lazy<IProductManagmentService> _productManagmentService;
    private readonly Lazy<IProductsQueryService> _productsQueryService;
    private readonly Lazy<IFarmerQueryService> _farmerQueryService;
    private readonly Lazy<IFarmerValidationService> _farmerValidationService;
    private readonly Lazy<IFarmerManagementService> _farmerManagementService;
    private readonly Lazy<IOrderManagmentService> _orderManagmentService;
    private readonly Lazy<IEcontManagmentService> _econtManagmentService;
    #endregion

    public ServiceManager(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILoggerManager loggerManager,
        IConfiguration configuration,
        HttpClient httpClient)
    {
        #region Service Extensions
        _productManagmentService = new Lazy<IProductManagmentService>(() => new ProductManagmentService(repositoryManager, loggerManager, mapper));
        _productsQueryService = new Lazy<IProductsQueryService>(() => new ProductsQueryService(repositoryManager, loggerManager, mapper));
        _farmerQueryService = new Lazy<IFarmerQueryService>(() => new FarmerQueryService(repositoryManager, loggerManager, mapper));
        _farmerValidationService = new Lazy<IFarmerValidationService>(() => new FarmerValidationService(repositoryManager));
        _farmerManagementService = new Lazy<IFarmerManagementService>(() => new FarmerManagementService(repositoryManager, loggerManager, mapper, _farmerValidationService.Value));
        _econtManagmentService = new Lazy<IEcontManagmentService> (() => new EcontManagmentService(repositoryManager, loggerManager, mapper));
        _orderManagmentService = new Lazy<IOrderManagmentService>(() => new OrderManagmentService(repositoryManager, loggerManager, mapper, _econtManagmentService.Value));
		#endregion

		_farmerService = new Lazy<IFarmerService>(() => new FarmerService(_farmerQueryService.Value, _farmerValidationService.Value, _farmerManagementService.Value));
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
        _productService = new Lazy<IProductService>(() => new ProductService(_productManagmentService.Value, _productsQueryService.Value));
        _productPhotoService = new Lazy<IProductPhotoService>(() => new ProductPhotoService(repositoryManager, loggerManager, mapper));
        _reviewService = new Lazy<IReviewService>(() => new ReviewService(repositoryManager, loggerManager, mapper));
        _cartService = new Lazy<ICartService>(() => new CartService(repositoryManager, loggerManager, mapper));
        _orderService = new Lazy<IOrderService>(() => new OrderService(repositoryManager, loggerManager, mapper, _orderManagmentService.Value, _econtManagmentService.Value));
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

    #region Service Extensions
    public IProductManagmentService ProductManagmentService => _productManagmentService.Value;

    public IProductsQueryService ProductsQueryService => _productsQueryService.Value;

    public IFarmerQueryService FarmerQueryService => _farmerQueryService.Value;

    public IFarmerValidationService FarmerValidationService => _farmerValidationService.Value;

    public IFarmerManagementService FarmerManagementService => _farmerManagementService.Value;

	public IOrderManagmentService OrderManagmentService => _orderManagmentService.Value;

	public IEcontManagmentService EcontManagmentService => _econtManagmentService.Value;
	#endregion
}
