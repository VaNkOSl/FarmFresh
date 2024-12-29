namespace EntityDataGenerator.Interfaces;

public interface IDataGenerator
{
    void GenerateFarmers(Dictionary<string, string>? options);

    void GenerateOrders(Dictionary<string, string>? options);
}
