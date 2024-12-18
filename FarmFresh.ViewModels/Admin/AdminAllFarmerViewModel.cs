namespace FarmFresh.ViewModels.Admin;

public abstract record AdminAllFarmerViewModel
{
    public Guid Id { get; set; }

    public string FarmerFullName { get; set; } = string.Empty;

    public string FarmDescription { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public byte[] Photo { get; set; } = new byte[0];

    public string Egn { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string PhotoString { get; set; } = string.Empty;
}
