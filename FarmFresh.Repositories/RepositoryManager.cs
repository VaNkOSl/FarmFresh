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
    private readonly Lazy<ICityRepository> _cityRepository;
    private readonly Lazy<IOfficeRepository> _officeRepository;
    private readonly Lazy<IAddressRespository> _addressRepository;
    private readonly Lazy<IStreetRepository> _streetRepository;
    private readonly Lazy<IQuarterRepository> _quarterRepository;
    private readonly Lazy<IProductRepository> _productRepository;
    private readonly Lazy<IProductPhotoRepository> _productPhotoRepository;
    private readonly Lazy<IReviewRepository> _reviewRepository;
    private readonly Lazy<IOrderRepository> _orderRepository;
    private readonly Lazy<IOrderProductRepository> _orderProductRepository;
    private readonly Lazy<ICartItemRepository> _cartItemRepository;
    private readonly IValidateEntity _validateEntityRepo;

    public RepositoryManager(FarmFreshDbContext data, IServiceProvider serviceProvider)
    {
        _data = data;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(data));
        _farmerRepository = new Lazy<IFarmerRepository>(() => new FarmerRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _farmerLocationRepository = new Lazy<IFarmerLocationRepository>(() => new FarmerLocationRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _countryRepository = new Lazy<ICountryRepository>(() => new CountryRepository(data));
        _cityRepository = new Lazy<ICityRepository>(() => new CityRepository(data));
        _addressRepository = new Lazy<IAddressRespository>(() => new AddressRepository(data));
        _officeRepository = new Lazy<IOfficeRepository>(() => new OfficeRepository(data, AddressRepository));
        _streetRepository = new Lazy<IStreetRepository>(() => new StreetRepository(data));
        _quarterRepository = new Lazy<IQuarterRepository>(() => new QuarterRepository(data));
        _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _productPhotoRepository = new Lazy<IProductPhotoRepository>(() => new ProductPhotoRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _reviewRepository = new Lazy<IReviewRepository>(() => new ReviewRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _orderProductRepository = new Lazy<IOrderProductRepository>(() => new OrderProductRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _cartItemRepository = new Lazy<ICartItemRepository>(() => new CartItemRepository(data, serviceProvider.GetRequiredService<IValidateEntity>()));
        _validateEntityRepo = serviceProvider.GetRequiredService<IValidateEntity>();
    }

    public IUserRepository UserRepository => _userRepository.Value;

    public IFarmerRepository FarmerRepository => _farmerRepository.Value;

    public IFarmerLocationRepository FarmerLocationRepository => _farmerLocationRepository.Value;

    public ICategoryRepository CategoryRepository => _categoryRepository.Value;

    public ICountryRepository CountryRepository => _countryRepository.Value;

    public ICityRepository CityRepository => _cityRepository.Value;

    public IOfficeRepository OfficeRepository => _officeRepository.Value;

    public IAddressRespository AddressRepository => _addressRepository.Value;

    public IStreetRepository StreetRepository => _streetRepository.Value;

    public IQuarterRepository QuarterRepository => _quarterRepository.Value;

    public IProductRepository ProductRepository => _productRepository.Value;

    public IProductPhotoRepository ProductPhotoRepository => _productPhotoRepository.Value;

    public IReviewRepository ReviewRepository => _reviewRepository.Value;

    public IOrderRepository OrderRepository => _orderRepository.Value;

    public IOrderProductRepository OrderProductRepository => _orderProductRepository.Value;

    public ICartItemRepository CartItemRepository => _cartItemRepository.Value;

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
