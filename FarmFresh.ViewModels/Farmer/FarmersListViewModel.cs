using FarmFresh.Commons.RequestFeatures;

namespace FarmFresh.ViewModels.Farmer;

public class FarmersListViewModel
{
    public IEnumerable<FarmersViewModel> Farmers { get; set; } = Enumerable.Empty<FarmersViewModel>();

    public MetaData MetaData { get; set; } = new MetaData();

    public string? SearchTerm { get; set; }
}
