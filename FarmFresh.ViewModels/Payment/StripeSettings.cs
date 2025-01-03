namespace FarmFresh.ViewModels.Payment;

public record class StripeSettings
{
    public string PublishableKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
}
