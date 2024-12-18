using EntityDataGenerator.Interfaces;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.Contacts;

namespace EntityDataGenerator;

public class DataGenerator : IDataGenerator
{
    private readonly HashSet<IEntityGenerator> _generators = [];
    private IRepositoryManager _repositoryManager;
    public DataGenerator(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;

        _generators.Add(new FarmerGenerator());
    }

    public async void GenerateFarmers(Dictionary<string, string>? options)
    {
        var entites = _generators.Single(x => x.GetType() == typeof(FarmerGenerator)).Generate(options);

        foreach(var entity in entites)
        {
            if (entity is Farmer farmer)
            {
                await _repositoryManager.FarmerRepository.CreateFarmerAsync(farmer);
                await _repositoryManager.SaveAsync(farmer);
            }
            else
            {
                throw new InvalidCastException($"Entity is not of type Farmer. Actual type: {entity.GetType()}");
            }
        }
    }
}
