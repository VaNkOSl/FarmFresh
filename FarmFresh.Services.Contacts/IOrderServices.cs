using FarmFresh.Data.Models.Enums;
using FarmFresh.ViewModels.Order;

namespace FarmFresh.Services.Contacts;

public interface IOrderService
{
    Task<Guid> CheckoutAsync(CreateOrderDto model, Guid userId, bool trackChanges);

    Task CompleteOrderAsync(Guid orderId, bool trackChanges);

    Task CompleteOrderAsync(Guid orderId, bool trackChanges, PaymentOption paymentOption);

    Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId, bool trackChanges);

    Task<List<OrderListViewModel>> GetOrdersForUserAsync(string userId, bool trackChanges);

    Task<OrderDetailsViewModel> GetOrderDetailsAsync(Guid id, bool trackChanges);

    Task<IEnumerable<FarmerOrderListViewModel>> GetOrderConfirmationForFarmersViewModelAsync(Guid farmerId, bool trackChanges);

    Task<bool> SendOrderAsync(Guid orderId, bool trackChanges);

    Task<bool> CancelOrder(Guid orderId, bool trackChanges);

    Task<IEnumerable<string>> GetCitiesAsync(string searchTerm);

    Task<IEnumerable<string>> GetEcontOfficesAsync(string cityName);
}
