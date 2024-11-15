using AutoMapper;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using LoggerService.Contacts;

namespace FarmFresh.Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IFarmerService> _farmerService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager)
    {
        _farmerService = new Lazy<IFarmerService>(() => new FarmerService(repositoryManager, loggerManager, mapper));
    }

    public IFarmerService FarmerService => _farmerService.Value;
}
