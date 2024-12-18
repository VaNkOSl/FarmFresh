using EntityDataGenerator.Interfaces;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;

namespace EntityDataGenerator;

public class FarmerGenerator : IEntityGenerator
{
    private static int _farmersCount;

    private readonly PhotoGenerator _photoGenerator = new();

    public IEnumerable<Entity_1<Guid>> Generate(Dictionary<string, string>? options)
    {
        if (options is null)
        {
            return new List<Farmer> { GenerateFarmers() };
        }

        var farmers = new List<Farmer>();

        foreach (var option in options)
        {
            switch (option.Key)
            {
                case "Count":
                    if (int.TryParse(option.Value, out var count))
                    {
                        AddFarmersToList(int.Parse(option.Value), farmers);
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid value for 'Count': {option.Value}");
                    }

                    break;


                default:
                    throw new ArgumentException($"Unknown option: {option.Key}");
            }
        }

        return farmers;
    }

    private Farmer GenerateFarmers()
    {
        var photo = _photoGenerator.Generate(300, 500);
        var farmer = new Farmer(
            Guid.NewGuid(),
            $"Description{_farmersCount}",
            $"FarmerLocation{_farmersCount}",
            $"088845688{_farmersCount}",
            photo,
            $"123456789{_farmersCount}",
            new DateTime(2000, 1, 1),
            Status.PendingApproval,
            Guid.NewGuid());

        return farmer;
    }

    private void AddFarmersToList(int count, List<Farmer> farmers)
    {
        for (int i = 0; i < count; i++)
        {
            farmers.Add(GenerateFarmers());
        }
    }
}
