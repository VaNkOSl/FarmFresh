using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using Microsoft.Extensions.DependencyInjection;

namespace FarmFresh.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly FarmFreshDbContext _data;
    private Lazy<IUserRepository> _userRepository;
    private Lazy<IFarmerRepository> _farmerRepository;
    private Lazy<IFarmerLocationRepository> _farmerLocationRepository;
    private Lazy<ICategoryRepository> _categoryRepository;
    private Lazy<IProductRepository> _productRepository;
    private Lazy<IProductPhotoRepository> _productPhotoRepository;
    private Lazy<IReviewRepository> _reviewRepository;
    private Lazy<IOrderRepository> _orderRepository;
    private IValidateEntity _validateEntityRepo;

    public RepositoryManager(FarmFreshDbContext data, IServiceProvider serviceProvider)
    {
        _data = data;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(data));
        _farmerRepository = new Lazy<IFarmerRepository>(() => new FarmerRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _farmerLocationRepository = new Lazy<IFarmerLocationRepository>(() => new FarmerLocationRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _productPhotoRepository = new Lazy<IProductPhotoRepository>(() => new ProductPhotoRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _reviewRepository = new Lazy<IReviewRepository>(() => new ReviewRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _orderRepository = new Lazy<IOrderRepository> (() => new OrderRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _validateEntityRepo = serviceProvider.GetRequiredService<IValidateEntity>();
    }

    public IUserRepository UserRepository => _userRepository.Value;

    public IFarmerRepository FarmerRepository => _farmerRepository.Value;

    public IFarmerLocationRepository FarmerLocationRepository => _farmerLocationRepository.Value;

    public ICategoryRepository CategoryRepository => _categoryRepository.Value;

    public IProductRepository ProductRepository => _productRepository.Value;

    public IProductPhotoRepository ProductPhotoRepository => _productPhotoRepository.Value;

    public IReviewRepository ReviewRepository => _reviewRepository.Value;

    public IOrderRepository OrderRepository => _orderRepository.Value;
    public async Task<CRUDResult> SaveAsync(Entity entity)
    {
        var validationResult =  _validateEntityRepo.Validate(entity);
        if (validationResult != CRUDResult.Success)
        {
            return CRUDResult.EntityValidationFailed;
        }

        await _data.SaveChangesAsync();
        
        return CRUDResult.Success;
    }

     public async Task SaveAsync() => await _data.SaveChangesAsync();    
}
