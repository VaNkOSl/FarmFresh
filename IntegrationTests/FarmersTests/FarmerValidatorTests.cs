using EntityDataGenerator;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IntegrationTests.FarmersTests;

public class FarmerValidatorTests
{
    private readonly FarmFreshDbContext _context;
    private readonly IRepositoryManager _repositoryManager;
    private readonly DataGenerator _generator;

    public FarmerValidatorTests()
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
    public async void CreateFarmer_ShouldAddFarmersSuccessfully()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        Assert.NotNull(farmer);
        Assert.Equal("Description0", farmer.FarmDescription);
        Assert.Equal("FarmerLocation0", farmer.Location);
        Assert.Equal("0888456880", farmer.PhoneNumber);
        Assert.Equal("1234567890", farmer.Egn);
        Assert.Equal(new DateTime(2000, 1, 1), farmer.DateOfBirth);
        Assert.Equal(Status.PendingApproval, farmer.FarmerStatus);
    }

    [Fact]
    public async void CreateFarmer_ShouldReturnEntityValidationFailed_When_Description_Is_More_Than_200_Characters()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        farmer.FarmDescription = new string('a', 300);

        var result = await _repositoryManager.SaveAsync(farmer);
        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateFarmer_ShouldReturnEntityValidationFailed_When_Description_Is_Equal_To_0_Characters()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        farmer.FarmDescription = new string('a', 0);

        var result = await _repositoryManager.SaveAsync(farmer);
        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateFarmer_ShouldReturnEntityValidationFailed_When_Location_Is_More_Than_100_Characters()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        farmer.Location = new string('a', 105);
        var result = await _repositoryManager.SaveAsync(farmer);
        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateFarmer_ShouldReturnEntityValidationFailed_When_Location_Is_Less_Than_10_Characters()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        farmer.Location = new string('a', 5);
        var result = await _repositoryManager.SaveAsync(farmer);
        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateFarmer_ShouldReturnEntityValidationFailed_When_PhoneNumber_Is_More_Than_14_Characters()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        farmer.PhoneNumber = new string('a', 15);
        var result = await _repositoryManager.SaveAsync(farmer);
        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateFarmer_ShouldReturnEntityValidationFailed_When_PhoneNumber_Is_Less_Than_7_Characters()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        farmer.PhoneNumber = new string('a', 6);
        var result = await _repositoryManager.SaveAsync(farmer);
        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateFarmer_ShouldReturnEntityValidationFailed_When_Egn_Is_More_Than_10_Characters()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        farmer.Egn = new string('0', 11);
        var result = await _repositoryManager.SaveAsync(farmer);
        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    [Fact]
    public async void CreateFarmer_ShouldReturnEntityValidationFailed_When_Egn_Is_Less_Than_10_Characters()
    {
        var farmer = await CreateFarmerAndGetFirstAsync();

        farmer.Egn = new string('0', 9);
        var result = await _repositoryManager.SaveAsync(farmer);
        Assert.Equal(CRUDResult.EntityValidationFailed, result);
    }

    private async Task<Farmer> CreateFarmerAndGetFirstAsync()
    {
        _generator.GenerateFarmers(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        return await _repositoryManager
            .FarmerRepository
            .FindAllFarmers(trackChanges: false)
            .FirstOrDefaultAsync();
    }
}
