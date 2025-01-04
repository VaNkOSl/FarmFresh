namespace FarmFresh.ViewModels.User;

public record class AllUserDto
{
    public string Id { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsSeller { get; set; }

    public string Email { get; set; } = string.Empty;
}
