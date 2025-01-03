using EntityDataGenerator.Interfaces;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;

namespace EntityDataGenerator.EntityGenerator;

public class OrderGenerator : IEntityGenerator
{
    private static int _ordersCount;
    public IEnumerable<Entity_1<Guid>> Generate(Dictionary<string, string>? options)
    {
        if(options is null)
        {
            return new List<Order> { GenerateOrders() };
        }

        var orders = new List<Order>();

        foreach (var option in options)
        {
            switch (option.Key)
            {
                case "Count":
                    if (int.TryParse(option.Value, out var count))
                    {
                        AddOrdersToList(int.Parse(option.Value), orders);
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

        return orders;
    }

    private Order GenerateOrders()
    {
        var order = new Order(
            Guid.NewGuid(),
            $"FirstName{_ordersCount}",
            $"LastName{_ordersCount}",
            $"Adress{_ordersCount}",
            $"88888888{_ordersCount}",
            $"myFirstOrder{_ordersCount}@abv.bg",
            DateTime.Now,
            DeliveryOption.Econt,
            OrderStatus.Cart,
            false,
            Guid.NewGuid());

        _ordersCount++;

        return order;
    }

    private void AddOrdersToList(int count, List<Order> orders)
    {
        for (int i = 0; i < count; i++)
        {
            orders.Add(GenerateOrders());
        }
    }
}
