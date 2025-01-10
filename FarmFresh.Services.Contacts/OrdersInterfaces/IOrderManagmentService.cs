using FarmFresh.ViewModels.Order;

namespace FarmFresh.Services.Contacts.OrdersInterfaces;

public interface IOrderManagmentService
{
	Task<Guid> CheckoutAsync(CreateOrderDto model, Guid userId, bool trackChanges);

	Task CompleteOrderAsync(Guid orderId, bool trackChanges);

	Task<bool> SendOrderAsync(Guid orderId, bool trackChanges);

	Task<bool> CancelOrder(Guid orderId, bool trackChanges);

	Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId, bool trackChanges);

	Task<IEnumerable<FarmerOrderListViewModel>> GetOrderConfirmationForFarmersViewModelAsync(Guid farmerId, bool trackChanges);

	Task<List<OrderListViewModel>> GetOrdersForUserAsync(string userId, bool trackChanges);
}
