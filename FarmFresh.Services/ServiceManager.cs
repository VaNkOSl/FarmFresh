using AutoMapper;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using LoggerService.Contacts;

namespace FarmFresh.Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IFarmerService> _farmerService;

    private readonly Lazy<IAdminService> _adminService;

    private readonly Lazy<ICategoryService> _categoryService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager)
    {
        _farmerService = new Lazy<IFarmerService>(() => new FarmerService(repositoryManager, loggerManager, mapper));
        _adminService = new Lazy<IAdminService>(() => new AdminService(repositoryManager, loggerManager, mapper));
        _categoryService = new Lazy<ICategoryService>(() => new CategoryService(repositoryManager, loggerManager, mapper));
    }

    public IFarmerService FarmerService => _farmerService.Value;

    public IAdminService AdminService => _adminService.Value;

    public ICategoryService CategoryService => _categoryService.Value;
}
