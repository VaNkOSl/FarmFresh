using EntityDataGenerator;
using FarmFresh.Data;
using FarmFresh.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using FarmFresh.Services.Contacts;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IntegrationTests.FarmersTests;

public class FarmerTests
{
    private readonly FarmFreshDbContext _context;
    private readonly IRepositoryManager _repositoryManager;
    private readonly DataGenerator _generator;
    private readonly IServiceManager _serviceManager;

    public FarmerTests()
    {
        var options = new DbContextOptionsBuilder<FarmFreshDbContext>()
            .UseInMemoryDatabase("FarmFreshInMemoryDataBase")
            .Options;

        _context = new FarmFreshDbContext(options);

        var serviceProviderMock = new Mock<IServiceProvider>();
        var validateEntityMock = new Mock<IValidateEntity>();

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IValidateEntity)))
            .Returns(validateEntityMock.Object);

        _repositoryManager = new RepositoryManager(_context, serviceProviderMock.Object);

        _generator = new DataGenerator(_repositoryManager);
    }

    [Fact]
    public void AddFarmers_ShouldAddFarmersSuccessfully()
    {
        _generator.GenerateFarmers(new Dictionary<string, string>
        {
             { "Count","1" }
        }); 
    }
}
