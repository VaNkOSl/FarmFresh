using AutoMapper;
using EntityDataGenerator;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories;
using FarmFresh.Repositories.DataValidator;
using FarmFresh.Services;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Order;
using LoggerService.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace IntegrationTests.OrderTests;

public class OrderServiceTests
{
	private readonly IServiceManager _serviceManager;
	private readonly Mock<ILoggerManager> _loggerManagerMock;
	private readonly IMapper _mapper;
	private readonly Mock<IConfiguration> _configurationMock;
	private readonly Mock<HttpClient> _httpClientMock;
	private readonly FarmFreshDbContext _context;
	private readonly DataGenerator _generator;
	private readonly IRepositoryManager _repositoryManager;

	public OrderServiceTests()
	{
		var options = new DbContextOptionsBuilder<FarmFreshDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		_context = new FarmFreshDbContext(options);

		_loggerManagerMock = new Mock<ILoggerManager>();
		_configurationMock = new Mock<IConfiguration>();
		_httpClientMock = new Mock<HttpClient>();

		var config = new MapperConfiguration(cfg =>
		{
			cfg.CreateMap<CreateOrderDto, Order>();
		});

		_mapper = config.CreateMapper();

		var serviceProviderMock = new Mock<IServiceProvider>();
		var crudDataValidator = new CRUDDataValidator();

		serviceProviderMock
			.Setup(sp => sp.GetService(typeof(IValidateEntity)))
			.Returns(crudDataValidator);

		_repositoryManager = new RepositoryManager(_context, serviceProviderMock.Object);

		_serviceManager = new ServiceManager(
			_repositoryManager,
			_mapper,
			_loggerManagerMock.Object,
			_configurationMock.Object,
			_httpClientMock.Object
		);

		_generator = new DataGenerator(_repositoryManager);

	}

	[Fact]
	public async Task CheckoutAsync_ShouldChckoutSuccessfully()
	{
		_generator.GenerateOrders(new Dictionary<string, string>
	    {
		    { "Count", "1" }
	    });

		var user = await _repositoryManager.UserRepository.GetAllUsers(trackChanges: true)
			.FirstOrDefaultAsync();

		var order = await _repositoryManager.OrderRepository.FindAllOrders(trackChanges: true)
			.FirstOrDefaultAsync();

		var model = new CreateOrderDto
		{
			Id = order.Id,
			FirstName = "Иван",
			LastName = "Петров",
			Adress = "София, София бул. Рожен №20E",
			Email = "ivan_petrov0341@abv.bg",
			PhoneNumber = "00059884265630",
			CreateOrderdDate = DateTime.UtcNow,
			DeliveryOption = 0,
			UserId = user.Id,
			User = user,
			City = "София",
			StreetName = "Студентски град стоян камбарев",
			StreetNum = "5",
			EcontOfficeAddress = "София, София бул. Рожен №20E"
		};

		var orderId = await _serviceManager.OrderService.CheckoutAsync(model, user.Id, trackChanges: true);

		Assert.NotNull(orderId);

		var orderFromDb = await _repositoryManager.OrderRepository
			.FindOrderByConditionAsync(o => o.Id == orderId, trackChanges: false)
			.FirstOrDefaultAsync();

		Assert.NotNull(orderFromDb);

		Assert.Equal(model.FirstName, orderFromDb.FirstName);
		Assert.Equal(model.LastName, orderFromDb.LastName);
		Assert.Equal(model.City, orderFromDb.City);
		Assert.Equal($"{model.City}, {model.EcontOfficeAddress}", orderFromDb.Adress);
		Assert.Equal(model.PhoneNumber, orderFromDb.PhoneNumber);
		Assert.Equal(model.Email, orderFromDb.Email);
		Assert.Equal(model.StreetName, orderFromDb.StreetName);
		Assert.Equal(model.StreetNum, orderFromDb.StreetNum);
	}
}
