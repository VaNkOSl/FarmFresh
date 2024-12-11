namespace FarmFresh.ViewModels.Farmer;

public class FarmersViewModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string PhotoString { get; set; } = string.Empty;

    public byte[] Photo {  get; set; } = new byte[0];

    public int ProductCount {  get; set; }
}
