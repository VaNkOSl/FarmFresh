using FarmFresh.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Services.Contacts;

public interface IOrderService
{
    Task<Guid> CheckoutAsync(CreateOrderDto model, Guid userId, bool trackChanges);

    Task CompleteOrderAsync(Guid orderId, bool trackChanges);

    Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId, bool trackChanges);

    Task<List<OrderListViewModel>> GetOrdersForUserAsync(Guid userId, bool trackChanges);

    Task<OrderDetailsViewModel> GetOrderDetailsAsync(Guid id, bool trackChanges);

    Task<IEnumerable<FarmerOrderListViewModel>> GetOrderConfirmationForFarmersViewModelAsync(Guid farmerId, bool trackChanges);

    Task<bool> SendOrderAsync(Guid orderId, bool trackChanges);

    Task<bool> CancelOrder(Guid orderId, bool trackChanges);
    Task<IEnumerable<string>> GetCitiesAsync(string searchTerm);
    Task<IEnumerable<string>> GetEcontOfficesAsync(string cityName);
}
