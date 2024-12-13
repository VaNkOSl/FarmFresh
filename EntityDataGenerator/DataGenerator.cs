using EntityDataGenerator.Interfaces;
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

    public void GenerateFarmers(Dictionary<string, string>? options)
    {
        var entites = _generators.Single(x => x.GetType() == typeof(FarmerGenerator)).Generate(options);

        foreach(var entity in entites)
        {
            _repositoryManager.SaveAsync(entity);
        }
    }
}
