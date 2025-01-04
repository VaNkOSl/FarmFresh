using EntityDataGenerator;
using FarmFresh.Data;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using FarmFresh.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;

namespace IntegrationTests.OrderTests;

public class OrderRepositoryTests
{
    private readonly FarmFreshDbContext _context;
    private readonly IRepositoryManager _repositoryManager;
    private readonly DataGenerator _generator;

    public OrderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FarmFreshDbContext>()
                    .UseInMemoryDatabase("FarmFreshInMemoryDataBase" + Guid.NewGuid())
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
    public async void CreateOrder_Should_Create_Order_Successfully()
    {
        var initialCount = await _repositoryManager.OrderRepository
            .FindAllOrders(trackChanges:false)
            .CountAsync();

        Assert.Equal(0, initialCount);

        _generator.GenerateOrders(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var realCount = await _repositoryManager.OrderRepository
            .FindAllOrders(trackChanges: false)
            .CountAsync();

        Assert.Equal(initialCount + 1, realCount);
    }

    [Fact]
    public async void GetOrderById_Should_Return_Correct_Order()
    {
        _generator.GenerateOrders(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var order = await _repositoryManager.OrderRepository
            .FindAllOrders(trackChanges: false)
            .FirstOrDefaultAsync();

        var retrivedOrder = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(f => f.Id == order.Id, trackChanges: false)
            .FirstOrDefaultAsync();

        Assert.Equal(order.Id, retrivedOrder.Id);
        Assert.Equal(order.FirstName, retrivedOrder.FirstName);
        Assert.Equal(order.LastName, retrivedOrder.LastName);
        Assert.Equal(order.PhoneNumber, retrivedOrder.PhoneNumber);
        Assert.Equal(order.Adress, retrivedOrder.Adress);
        Assert.Equal(order.DeliveryOption, retrivedOrder.DeliveryOption);
    }

    [Fact]
    public async void UpdateOrder_Should_Update_Order_Successfully()
    {
        _generator.GenerateOrders(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var order = await _repositoryManager.OrderRepository
           .FindAllOrders(trackChanges: true)
           .FirstOrDefaultAsync();

        Assert.NotNull(order);
        Assert.Equal("FirstName0", order.FirstName);
        Assert.Equal("LastName0", order.LastName);
        Assert.Equal("Adress0", order.Adress);
        Assert.Equal("888888880", order.PhoneNumber);
        Assert.Equal("myFirstOrder0@abv.bg", order.Email);

        order.FirstName = "UpdateName";
        order.LastName = "UpdateLastName";
        order.Adress = "UpdateAdress";
        order.PhoneNumber = "00000000";

        _repositoryManager.OrderRepository.UpdateOrder(order);

        var result = await _repositoryManager.SaveAsync(order);

        Assert.Equal(CRUDResult.Success, result);
        Assert.Equal("UpdateName", order.FirstName);
        Assert.Equal("UpdateLastName", order.LastName);
        Assert.Equal("UpdateAdress", order.Adress);
        Assert.Equal("00000000", order.PhoneNumber);
    }

    [Fact]
    public async void DeleteOrder_Should_Delete_Order_Successfully()
    {
        _generator.GenerateOrders(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var order = await _repositoryManager.OrderRepository
           .FindAllOrders(trackChanges: true)
           .FirstOrDefaultAsync();

        var countBeforeDeleting = await _repositoryManager.OrderRepository
            .FindAllOrders(trackChanges: false)
            .CountAsync();

        Assert.Equal(1, countBeforeDeleting);

        _repositoryManager.OrderRepository.DeleteOrder(order);
        await _repositoryManager.SaveAsync(order);

        var countAfterDeleting = await _repositoryManager.OrderRepository
            .FindAllOrders(trackChanges: false)
            .CountAsync();

        Assert.Equal(0, countAfterDeleting);
    }
}
