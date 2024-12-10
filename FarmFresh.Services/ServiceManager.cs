using AutoMapper;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
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

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager, IConfiguration configuration)
    {
        _farmerService = new Lazy<IFarmerService>(() => new FarmerService(repositoryManager, loggerManager, mapper));
        _adminService = new Lazy<IAdminService>(() => new AdminService(repositoryManager, loggerManager, mapper, configuration));
        _categoryService = new Lazy<ICategoryService>(() => new CategoryService(repositoryManager, loggerManager, mapper));
        _productService = new Lazy<IProductService>(() => new ProductService(repositoryManager, loggerManager, mapper));
        _productPhotoService = new Lazy<IProductPhotoService>(() => new ProductPhotoService(repositoryManager, loggerManager, mapper));
    }

    public IFarmerService FarmerService => _farmerService.Value;

    public IAdminService AdminService => _adminService.Value;

    public ICategoryService CategoryService => _categoryService.Value;

    public IProductService ProductService => _productService.Value;

    public IProductPhotoService ProductPhotoService => _productPhotoService.Value;
}
