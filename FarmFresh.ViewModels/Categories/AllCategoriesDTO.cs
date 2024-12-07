namespace FarmFresh.ViewModels.Categories;

public record class AllCategoriesDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int ProductCount { get; init; }

    public AllCategoriesDTO() 
    {
    }
    public AllCategoriesDTO(Guid id, string name, int productCount)
    {
        Id = id;
        Name = name;
        ProductCount = productCount;
    }
}
