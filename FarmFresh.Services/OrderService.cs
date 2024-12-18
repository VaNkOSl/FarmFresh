using AutoMapper;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Order;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace FarmFresh.Services;

internal class OrderService : IOrderService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public OrderService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }


    public async Task<Guid> CheckoutAsync(CreateOrderDto model, Guid userId, bool trackChanges)
    {

        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(u => u.UserId == userId, trackChanges)
            .Include(ph => ph.OrderProducts)
            .ThenInclude(p => p.Product)
            .ThenInclude(ph => ph.ProductPhotos)
            .FirstOrDefaultAsync();

        order.FirstName = model.FirstName;
        order.LastName = model.LastName;
        order.PhoneNumber = model.PhoneNumber;
        order.Email = model.Email;
        order.DeliveryOption = model.DeliveryOption;
        order.Adress = model.Adress;

        foreach (var item in order.OrderProducts)
        {
            var product = item.Product;

            foreach(var photo in product.ProductPhotos)
            {
                order.ProductPhotos.Add(photo);
            }
        }

        _repositoryManager.OrderRepository.UpdateOrder(order);
        await _repositoryManager.SaveAsync(order);
        return order.Id;
    }

    public async Task CompleteOrderAsync(Guid orderId, bool trackChanges)
    {
        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(r => r.Id == orderId, trackChanges)
            .FirstOrDefaultAsync();

        var cartItemToRemove = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(c => c.UserId == order.UserId, trackChanges)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        _repositoryManager.CartItemRepository.DeleteItem(cartItemToRemove);
        order.OrderStatus = OrderStatus.Completed;
        _repositoryManager.OrderRepository.UpdateOrder(order);
        await _repositoryManager.SaveAsync();
    }
    public async Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId, bool trackChanges)
    {
        var order = await _repositoryManager.OrderRepository
                                       .FindOrderByConditionAsync(o => o.Id == orderId, trackChanges)
                                       .Include(o => o.OrderProducts)
                                       .ThenInclude(p => p.Product)
                                       .ThenInclude(ph => ph.ProductPhotos)
                                       .FirstOrDefaultAsync();
        if (order == null) return null;

        var cartItems = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(c => c.UserId == order.UserId, trackChanges)
            .Include(p => p.Product)
            .Include(u => u.User)
            .ToListAsync();

        var cartItemViewModels = _mapper.Map<IEnumerable<CartItemViewModel>>(cartItems);

        return new OrderConfirmationViewModel(
          Id: order.Id,
          Price: order.OrderProducts != null && order.OrderProducts.Any()
              ? order.OrderProducts.Sum(p => p.Price)
              : 0,
          Quantity: order.OrderProducts != null && order.OrderProducts.Any()
              ? order.OrderProducts.Sum(p => p.Quantity)
              : 0,
          TotalPrice: order.OrderProducts != null && order.OrderProducts.Any()
              ? order.OrderProducts.Sum(p => p.Price * p.Quantity)
              : 0,
          FirstName: order.FirstName,
          LastName: order.LastName,
          Adress: order.Adress,
          PhoneNumber: order.PhoneNumber,
          Email: order.Email,
          Products: order.OrderProducts.ToList(),
          CartItems: cartItemViewModels,
          Photos: order.OrderProducts
              .SelectMany(op => op.Product.ProductPhotos)
              .Select(pp => new ProductPhotosDto(
                  pp.Id,
                  "/uploads/" + Path.GetFileName(pp.FilePath),
                  pp.Photo,
                  pp.ProductId
              ))
              .ToList()
      );
    }

    public async Task<List<OrderListViewModel>> GetOrdersForUserAsync(Guid userId, bool trackChanges)
    {
        var orderProducts = await _repositoryManager.OrderRepository
                .FindAllOrders(trackChanges) 
                .GetOrderProductsByUserId(userId)
                .Include(p => p.Product)
                .ThenInclude(ph => ph.ProductPhotos)
                .ToListAsync();

        return _mapper.Map<List<OrderListViewModel>>(orderProducts);
    }

    public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(Guid id, bool trackChanges)
    {
        var orderProduct = await _repositoryManager.OrderProductRepository
                  .FindAllOrderProducts(trackChanges)
                  .GetOrderProductDetailsById(id)
                  .FirstOrDefaultAsync();

        if (orderProduct == null)
        {
            return null;
        }

       return _mapper.Map<OrderDetailsViewModel>(orderProduct);
    }

    public async Task<IEnumerable<FarmerOrderListViewModel>> GetOrderConfirmationForFarmersViewModelAsync(Guid farmerId, bool trackChanges)
    {
        var orderProducts = await _repositoryManager.OrderRepository
          .FindAllOrders(trackChanges)
          .GetOrderProductsByFarmerId(farmerId)
		  .ToListAsync();

	    return _mapper.Map<IEnumerable<FarmerOrderListViewModel>>(orderProducts);
    }

    public async Task<bool> SendOrderAsync(Guid orderId, bool trackChanges)
    {
        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(o => o.Id == orderId, trackChanges)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return false;
        }

        if (order.OrderStatus == OrderStatus.Shipped)
        {
            return false;
        }

        order.OrderStatus = OrderStatus.Shipped;
        _repositoryManager.OrderRepository.UpdateOrder(order);
        await _repositoryManager.SaveAsync();
        return true;
    }

    public async Task<bool> CancelOrder(Guid orderId, bool trackChanges)
    {
        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(o => o.Id == orderId, trackChanges)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return false;
        }

        order.OrderStatus = OrderStatus.Canceled;
        _repositoryManager.OrderRepository.UpdateOrder(order);
        await _repositoryManager.SaveAsync();
        return true;
    }
}
public static class SessionExtension
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

}
