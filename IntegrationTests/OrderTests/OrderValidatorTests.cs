using EntityDataGenerator;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories;
using FarmFresh.Repositories.DataValidator;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IntegrationTests.OrderTests;

public class OrderValidatorTests
{
    private readonly FarmFreshDbContext _context;
    private readonly IRepositoryManager _repositoryManager;
    private readonly DataGenerator _generator;

    public OrderValidatorTests()
    {
        var options = new DbContextOptionsBuilder<FarmFreshDbContext>()
                    .UseInMemoryDatabase("FarmFreshInMemoryDataBase")
                    .Options;

        _context = new FarmFreshDbContext(options);

        var serviceProviderMock = new Mock<IServiceProvider>();

        var crudDataValidator = new CRUDDataValidator();

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IValidateEntity)))
            .Returns(crudDataValidator);

        _repositoryManager = new RepositoryManager(_context, serviceProviderMock.Object);

        _generator = new DataGenerator(_repositoryManager);
    }

    [Fact]
    public async void CreateOrder_ShouldCreateOrderSuccessfully()
    {
        var order = await CreateOrderAndGetFirstAsync();

        Assert.NotNull(order);
        Assert.Equal("FirstName0", order.FirstName);
        Assert.Equal("LastName0", order.LastName);
        Assert.Equal("Adress0", order.Adress);
        Assert.Equal("888888880", order.PhoneNumber);
        Assert.Equal("myFirstOrder0@abv.bg", order.Email);

        Assert.NotNull(order.User);
        Assert.Equal("userName0", order.User.UserName);
        Assert.Equal("example@abv.bg0", order.User.Email);
        Assert.Equal("FirstName0", order.User.FirstName);
        Assert.Equal("LastName0", order.User.LastName);
    }

    [Fact]
    public async void CreateOrder_ShouldReturnEntityValidationFailed_When_FirstName_Is_More_Than_15_Characters()
    {
        var order = await CreateOrderAndGetFirstAsync();

        order.FirstName = new string('a', 20);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateOrder_ShouldReturnEntityValidationFailed_When_FirstName_Is_Equal_To_1_Characters()
    {
        var order = await CreateOrderAndGetFirstAsync();

        order.FirstName = new string('a', 1);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateOrder_ShouldReturnEntityValidationFailed_When_LastNameIs_More_Than_15_Characters()
    {
        var order = await CreateOrderAndGetFirstAsync();

        order.LastName = new string('a', 20);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateOrder_ShouldReturnEntityValidationFailed_When_LastName_Is_Equal_To_2_Characters()
    {
        var order = await CreateOrderAndGetFirstAsync();

        order.LastName = new string('a', 2);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateOrder_ShouldReturnEntityValidationFailed_When_Adress_Is_More_Than_150_Characters()
    {
        var order = await CreateOrderAndGetFirstAsync();

        order.Adress = new string('a', 155);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateOrder_ShouldReturnEntityValidationFailed_When_Adress_Is_Equal_To_5_Characters()
    {
        var order = await CreateOrderAndGetFirstAsync();

        order.Adress = new string('a', 5);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateOrder_ShouldReturnEntityValidationFailed_When_PhoneNumber_Is_More_Than_14_Characters()
    {
        var order = await CreateOrderAndGetFirstAsync();

        order.PhoneNumber = new string('0', 15);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateOrder_ShouldReturnEntityValidationFailed_When_PhoneNumber_Is_Equal_To_6_Characters()
    {
        var order = await CreateOrderAndGetFirstAsync();

        order.PhoneNumber = new string('0', 6);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    private async Task<Order> CreateOrderAndGetFirstAsync()
    {
        _generator.GenerateOrders(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var orders = await _repositoryManager
        .OrderRepository
        .FindAllOrders(trackChanges: false)
        .Include(u => u.User)
        .ToListAsync();

        Assert.NotEmpty(orders);

        return orders.FirstOrDefault();
    }
}
