namespace FarmFresh.Data.Models;

public abstract class Entity_1<TKey> : Entity
{
    public TKey? Id { get; set; }

    public override bool Equals(object? obj) => (obj as Entity_1<TKey>)?.Id!.Equals(Id) ?? false;

    public override int GetHashCode() => Id!.GetHashCode();
}
