using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Data.Models.Repositories.Econt;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using FarmFresh.Repositories.Econt;
using LoggerService.Exceptions.BadRequest;
using Microsoft.Extensions.DependencyInjection;

namespace FarmFresh.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly FarmFreshDbContext _data;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IFarmerRepository> _farmerRepository;
    private readonly Lazy<IFarmerLocationRepository> _farmerLocationRepository;
    private readonly Lazy<ICategoryRepository> _categoryRepository;
    private readonly Lazy<ICountryRepository> _countryRepository;
    private readonly IValidateEntity _validateEntityRepo;

    public RepositoryManager(FarmFreshDbContext data, IServiceProvider serviceProvider)
    {
        _data = data;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(data));
        _farmerRepository = new Lazy<IFarmerRepository>(() => new FarmerRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _farmerLocationRepository = new Lazy<IFarmerLocationRepository>(() => new FarmerLocationRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _countryRepository = new Lazy<ICountryRepository>(() => new CountryRepository(data));
        _validateEntityRepo = serviceProvider.GetRequiredService<IValidateEntity>();
    }

    public IUserRepository UserRepository => _userRepository.Value;

    public IFarmerRepository FarmerRepository => _farmerRepository.Value;

    public IFarmerLocationRepository FarmerLocationRepository => _farmerLocationRepository.Value;

    public ICategoryRepository CategoryRepository => _categoryRepository.Value;

    public ICountryRepository CountryRepository => _countryRepository.Value;

    public async Task SaveAsync(Entity entity)
    {
        var validationResult =  _validateEntityRepo.Validate(entity);
        if (validationResult != CRUDResult.Success)
        {
            throw new EntityValidationException();
        }

        await _data.SaveChangesAsync();
    }

     public async Task SaveAsync() => await _data.SaveChangesAsync();    
}
