using FarmFresh.Data.Models;

namespace EntityDataGenerator.Interfaces;

public interface IEntityGenerator
{
    IEnumerable<Entity_1<Guid>> Generate(Dictionary<string, string>? options);
}
