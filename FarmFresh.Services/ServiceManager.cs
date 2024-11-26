using AutoMapper;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using LoggerService.Contacts;

namespace FarmFresh.Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IFarmerService> _farmerService;
    private readonly Lazy<IFarmerLocationService> _farmerlocationService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager)
    {
        _farmerService = new Lazy<IFarmerService>(() => new FarmerService(repositoryManager, loggerManager, mapper));
        _farmerlocationService = new Lazy<IFarmerLocationService>(() => new FarmerLocationService(repositoryManager, loggerManager, mapper));
    }

    public IFarmerService FarmerService => _farmerService.Value;

    public IFarmerLocationService FarmerLocationService => _farmerlocationService.Value;
}
