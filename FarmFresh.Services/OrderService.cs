using AutoMapper;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Order;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using Microsoft.EntityFrameworkCore;

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
            .GetOrderWithDetails()
            .FirstOrDefaultAsync();

        OrderHelper.ChekOrderNotFound(order, order.Id, "CheckoutAsync", _loggerManager);

        _mapper.Map(model, order);

        foreach (var item in order.OrderProducts)
        {
            var product = item.Product;

            foreach(var photo in product.ProductPhotos)
            {
                order.ProductPhotos.Add(photo);
            }
        }

        try
        {
            _repositoryManager.OrderRepository.UpdateOrder(order);
            await _repositoryManager.SaveAsync(order);
            return order.Id;
        }
        catch (Exception)
        {

            throw;
        }

    }

    public async Task CompleteOrderAsync(Guid orderId, bool trackChanges)
    {
        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(r => r.Id == orderId, trackChanges)
            .FirstOrDefaultAsync();

        OrderHelper.ChekOrderNotFound(order, order.Id, "CompleteOrderAsync", _loggerManager);

        var cartItemToRemove = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(c => c.UserId == order.UserId, trackChanges)
            .ToListAsync();

        foreach(var cart in cartItemToRemove)
        {
            _repositoryManager.CartItemRepository.DeleteItem(cart);
        }

        //CartHelper.CheckCartItemNotFound(cartItemToRemove, cartItemToRemove.Id, "CompleteOrderAsync", _loggerManager);

        order.OrderStatus = OrderStatus.Completed;
        _repositoryManager.OrderRepository.UpdateOrder(order);
        await _repositoryManager.SaveAsync();
    }
    public async Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId, bool trackChanges)
    {
        var order = await _repositoryManager.OrderRepository
                                       .FindOrderByConditionAsync(o => o.Id == orderId, trackChanges)
                                       .GetOrderWithDetails()
                                       .FirstOrDefaultAsync();

        OrderHelper.ChekOrderNotFound(order, order.Id, "GetOrderConfirmationViewModelAsync", _loggerManager);


        var cartItems = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(c => c.UserId == order.UserId, trackChanges)
            .Include(p => p.Product)
            .ThenInclude(ph => ph.ProductPhotos)
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
          .SelectMany(op => op.Product.ProductPhotos) // Събираме всички снимки на продуктите
          .Select(pp => new ProductPhotosDto(
              pp.Id,
              "/uploads/" + Path.GetFileName(pp.FilePath), // Връщаме пътя до снимката
              pp.Photo,
              pp.ProductId
          ))
          .ToList()
  );
        //  return new OrderConfirmationViewModel(
        //    Id: order.Id,
        //    Price: order.OrderProducts != null && order.OrderProducts.Any()
        //        ? order.OrderProducts.Sum(p => p.Price)
        //        : 0,
        //    Quantity: order.OrderProducts != null && order.OrderProducts.Any()
        //        ? order.OrderProducts.Sum(p => p.Quantity)
        //        : 0,
        //    TotalPrice: order.OrderProducts != null && order.OrderProducts.Any()
        //        ? order.OrderProducts.Sum(p => p.Price * p.Quantity)
        //        : 0,
        //    FirstName: order.FirstName,
        //    LastName: order.LastName,
        //    Adress: order.Adress,
        //    PhoneNumber: order.PhoneNumber,
        //    Email: order.Email,
        //    Products: order.OrderProducts.ToList(),
        //    CartItems: cartItemViewModels,
        //    Photos: order.OrderProducts
        //        .SelectMany(op => op.Product.ProductPhotos)
        //        .Select(pp => new ProductPhotosDto(
        //            pp.Id,
        //            "/uploads/" + Path.GetFileName(pp.FilePath),
        //            pp.Photo,
        //            pp.ProductId
        //        ))
        //        .ToList()
        //);
    }

    public async Task<List<OrderListViewModel>> GetOrdersForUserAsync(Guid userId, bool trackChanges)
    {
        var order = await _repositoryManager.OrderRepository
                .FindAllOrders(trackChanges) 
                .GetOrderProductsByUserId(userId)
                .Include(p => p.Product)
                .ThenInclude(ph => ph.ProductPhotos)
                .ToListAsync();

        return _mapper.Map<List<OrderListViewModel>>(order);
    }

    public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(Guid id, bool trackChanges)
    {
        var orderProduct = await _repositoryManager.OrderProductRepository
                  .FindAllOrderProducts(trackChanges)
                  .GetOrderProductDetailsById(id)
                  .FirstOrDefaultAsync();

       OrderProductHelper.CheckOrderProductNotFound(orderProduct, orderProduct.Id, "GetOrderDetailsAsync", _loggerManager);
       return _mapper.Map<OrderDetailsViewModel>(orderProduct);
    }

    public async Task<IEnumerable<FarmerOrderListViewModel>> GetOrderConfirmationForFarmersViewModelAsync(Guid farmerId, bool trackChanges)
    {
        var orderProducts = await _repositoryManager.OrderRepository
          .FindAllOrders(trackChanges)
          .GetOrderProductsByFarmerId(farmerId)
          .Include(p => p.Product)
          .ThenInclude(ph => ph.ProductPhotos)
		  .ToListAsync();

	    return _mapper.Map<IEnumerable<FarmerOrderListViewModel>>(orderProducts);
    }

    public async Task<bool> SendOrderAsync(Guid orderId, bool trackChanges)
    {
        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(o => o.Id == orderId, trackChanges)
            .FirstOrDefaultAsync();

        OrderHelper.ChekOrderNotFound(order, order.Id, "SendOrderAsync", _loggerManager);

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

        OrderHelper.ChekOrderNotFound(order, order.Id, "CancelOrder", _loggerManager);

        order.OrderStatus = OrderStatus.Canceled;
        _repositoryManager.OrderRepository.UpdateOrder(order);
        await _repositoryManager.SaveAsync();
        return true;
    }
}
