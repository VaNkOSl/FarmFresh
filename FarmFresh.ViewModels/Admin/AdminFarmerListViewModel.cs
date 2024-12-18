using FarmFresh.Commons.RequestFeatures;

namespace FarmFresh.ViewModels.Admin;

public record AdminFarmerListViewModel
{
    public IEnumerable<AdminAllFarmersDto> Farmers { get; set; } = Enumerable.Empty<AdminAllFarmersDto>();

    public MetaData MetaData { get; set; } = new MetaData();

    public string? SearchTerm { get; set; }
}
