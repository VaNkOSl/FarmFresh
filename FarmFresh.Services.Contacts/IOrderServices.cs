using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Order;

namespace FarmFresh.Services.Contacts
{
    public interface IOrderService
    {
        Task<Guid> CheckoutAsync(Order order, Guid orderProductId, Guid userId);
        Task CompleteOrderAsync(Guid orderId);
        Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId);
        Task<List<OrderListViewModel>> GetOrdersForUserAsync(Guid userId);

        Task<OrderDetailsViewModel> GetOrderDetailsAsync(Guid id);
    }
}
