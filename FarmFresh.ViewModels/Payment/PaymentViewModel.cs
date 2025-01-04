namespace FarmFresh.ViewModels.Payment;

public class PaymentViewModel
{
    public string StripeToken { get; set; }

    public decimal Amount { get; init; }

    public Guid OrderId { get; set; }
}
