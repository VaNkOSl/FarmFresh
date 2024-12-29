using EntityDataGenerator.EntityGenerator;
using EntityDataGenerator.Interfaces;
using FarmFresh.Data.Models;
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
        _generators.Add(new OrderGenerator());
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
                throw new ArgumentException($"Entity is not of type Farmer. Actual type: {entity.GetType()}");
            }
        }
    }

    public async void GenerateOrders(Dictionary<string, string>? options)
    {
        var entites = _generators.Single(x => x.GetType() == typeof(OrderGenerator)).Generate(options);

        foreach(var entity in entites)
        {
            if(entity is Order order)
            {
                var users = await GenerateUsers(1);

                if (users.Any())
                {
                    var user = users.First();
                    order.UserId = user.Id;
                    order.User = user;
                }

                await _repositoryManager.OrderRepository.AddOrderAsync(order);
                await _repositoryManager.SaveAsync(order);
            }
            else
            {
                throw new ArgumentException($"Entity is not of type Order. Actual type: {entity.GetType()}");
            }
        }

    }

    private async Task<IEnumerable<ApplicationUser>> GenerateUsers(int count)
    {
        var users = new List<ApplicationUser>();

        for (int i = 0; i < count; i++)
        {
            var user = new ApplicationUser(
                Guid.NewGuid(),
                $"userName{i}",
                $"example@abv.bg{i}",
                $"FirstName{i}",
                $"LastName{i}",
                DateTime.Now);

            await _repositoryManager.UserRepository.CreateUserAsync(user);
            await _repositoryManager.SaveAsync();

            users.Add(user);
        }

        return users;
    }

}
