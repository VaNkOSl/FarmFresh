using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Order;

public record FarmerOrderListViewModel(
	Guid Id,
	Guid OrderId,
	string ProductName,
	string UserFullName,
	decimal Price,
	int Quantity,
	ProductPhotosDto Photos);