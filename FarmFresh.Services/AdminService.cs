using AutoMapper;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using LoggerService.Contacts;

namespace FarmFresh.Services;

internal sealed class AdminService : IAdminService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public AdminService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }
}
