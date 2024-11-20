namespace FarmFresh.ViewModels.Farmer;

public class FarmerLocationDto
{
    public required double Latitude { get; set; }

    public required double Longitude { get; set; }

    public required string Title { get; set; } = string.Empty;

    public required Guid FarmerId { get; set; }
}
