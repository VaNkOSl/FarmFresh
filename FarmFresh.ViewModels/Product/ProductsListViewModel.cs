using FarmFresh.Commons.RequestFeatures;

namespace FarmFresh.ViewModels.Product;

public class ProductsListViewModel
{
    public IEnumerable<AllProductsDto> Products { get; set; } = Enumerable.Empty<AllProductsDto>();

    public MetaData MetaData { get; set; } = new MetaData();

    public string? SearchTerm { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }
}
