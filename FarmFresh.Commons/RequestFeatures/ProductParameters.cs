namespace FarmFresh.Commons.RequestFeatures;

public class ProductParameters : RequestParameters
{
    public decimal MinPrice { get; set; }

    public decimal MaxPrice { get; set; } = int.MaxValue;

    public bool ValidatePricerange => MaxPrice > MinPrice;

    public string? SearchTerm { get; set; }

    public string? OrderByPrice { get; set; }
}
