using FarmFresh.ViewModels.Order;

namespace FarmFresh.Services.Contacts;

public interface IOrderService
{
    Task<Guid> CheckoutAsync(CreateOrderDto model, Guid userId, bool trackChanges);

    Task CompleteOrderAsync(Guid orderId, bool trackChanges);

    Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId, bool trackChanges);

    Task<List<OrderListViewModel>> GetOrdersForUserAsync(Guid userId, bool trackChanges);

    Task<OrderDetailsViewModel> GetOrderDetailsAsync(Guid id, bool trackChanges);
}
