using EntityDataGenerator;
using FarmFresh.Data;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IntegrationTests.FarmersTests;

public class FarmerRepositoryTests
{
    private readonly FarmFreshDbContext _context;
    private readonly IRepositoryManager _repositoryManager;
    private readonly DataGenerator _generator;

    public FarmerRepositoryTests()
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
    public async void CreateFarmer_Should_Create_Farmer_Successfully()
    {
        var initialCount = await
            _repositoryManager
            .FarmerRepository
            .FindAllFarmers(trackChanges: false)
            .CountAsync();

        Assert.Equal(0, initialCount);

        _generator.GenerateFarmers(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var realCount = await
            _repositoryManager
            .FarmerRepository
            .FindAllFarmers(trackChanges: false)
            .CountAsync();

        Assert.Equal(initialCount + 1, realCount);
    }

    [Fact]
    public async void GetFarmerById_Should_Return_Correct_Farmer()
    {
        _generator.GenerateFarmers(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindAllFarmers(trackChanges: false)
            .FirstOrDefaultAsync();

        var retrievedFarmer = await
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.Id == farmer.Id, trackChanges: false)
            .FirstOrDefaultAsync();

        Assert.Equal(farmer.Id, retrievedFarmer.Id);
        Assert.Equal(farmer.FarmDescription, retrievedFarmer.FarmDescription);
        Assert.Equal(farmer.Location, retrievedFarmer.Location);
        Assert.Equal(farmer.Egn, retrievedFarmer.Egn);
        Assert.Equal(farmer.PhoneNumber, retrievedFarmer.PhoneNumber);
        Assert.Equal(farmer.Photo, retrievedFarmer.Photo);
        Assert.Equal(farmer.DateOfBirth, retrievedFarmer.DateOfBirth);
        Assert.Equal(farmer.UserId, retrievedFarmer.UserId);
    }

    [Fact]
    public async void UpdateFarmer_Should_Update_Farmer_Successfully()
    {
        _generator.GenerateFarmers(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindAllFarmers(trackChanges: true)
            .FirstOrDefaultAsync();

        Assert.NotNull(farmer);
        Assert.Equal("Description0", farmer.FarmDescription);
        Assert.Equal("FarmerLocation0", farmer.Location);
        Assert.Equal("0888456880", farmer.PhoneNumber);
        Assert.Equal("1234567890", farmer.Egn);

        farmer.FarmDescription = "DescriptionAfterUpdate";
        farmer.Location = "LocationAfterUpdate";
        farmer.PhoneNumber = "00000000";
        farmer.Egn = "0987654321";

        _repositoryManager.FarmerRepository.UpdateFarmer(farmer);
        var result = await _repositoryManager.SaveAsync(farmer);

        Assert.Equal(CRUDResult.Success, result);
        Assert.Equal("DescriptionAfterUpdate", farmer.FarmDescription);
        Assert.Equal("LocationAfterUpdate", farmer.Location);
        Assert.Equal("00000000", farmer.PhoneNumber);
        Assert.Equal("0987654321", farmer.Egn);
    }

    [Fact]
    public async void DeleteFarmer_Should_Delete_Farmer_Successfully()
    {
        _generator.GenerateFarmers(new Dictionary<string, string>
        {
            {"Count", "1" }
        });

        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindAllFarmers(trackChanges: true)
            .FirstOrDefaultAsync();

        var countBeforeDeleting = await
            _repositoryManager
            .FarmerRepository
            .FindAllFarmers(trackChanges: false)
            .CountAsync();

        Assert.Equal(1, countBeforeDeleting);

        _repositoryManager.FarmerRepository.DeleteFarmer(farmer);
        await _repositoryManager.SaveAsync(farmer);

        var countAfterDeleting = await
           _repositoryManager
           .FarmerRepository
           .FindAllFarmers(trackChanges: false)
           .CountAsync();

        Assert.Equal(0, countAfterDeleting);
    }
}
