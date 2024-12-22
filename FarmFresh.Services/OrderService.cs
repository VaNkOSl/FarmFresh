using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Econt.APIServices;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Order;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.CartItems;
using LoggerService.Exceptions.InternalError.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using static FarmFresh.Commons.EntityValidationConstants;
using static FarmFresh.Commons.MessagesConstants;

namespace FarmFresh.Services;

internal class OrderService : IOrderService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public OrderService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper
                        )
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

        try
        {
            model.Adress = $"{model.City}, {model.EcontOfficeAddress}";
            _loggerManager.LogInfo($"[{nameof(CheckoutAsync)}] Combined City and Econt Office Address into Address: {model.Adress}");

            _mapper.Map(model, order);
            _loggerManager.LogInfo($"[{CheckoutAsync}] Successfully mapped order data from CreateOrderDto to Order entity");

            foreach (var item in order.OrderProducts)
            {
                var product = item.Product;

                foreach (var photo in product.ProductPhotos)
                {
                    order.ProductPhotos.Add(photo);
                    _loggerManager.LogInfo($"[{CheckoutAsync}] Added photo with ID {photo.Id} to order {order.Id}");
                }
            }
            _repositoryManager.OrderRepository.UpdateOrder(order);
            await _repositoryManager.SaveAsync(order);
            _loggerManager.LogInfo($"[{CheckoutAsync}] Successfully updated order with ID {order.Id}");
            return order.Id;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"[{CheckoutAsync}] An error occurred while processing order for user {userId}: {ex.Message}");
            throw new OrderCheckoutInternalServiceError();
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

        _loggerManager.LogInfo($"[{CompleteOrderAsync}] Found {cartItemToRemove.Count} cart items to remove for user {order.UserId}");

        foreach (var cart in cartItemToRemove)
        {
            _repositoryManager.CartItemRepository.DeleteItem(cart);
            _loggerManager.LogInfo($"[{CompleteOrderAsync}] Removed cart item with ID {cart.Id} for user {order.UserId}");
        }

        order.OrderStatus = OrderStatus.Completed;
        _repositoryManager.OrderRepository.UpdateOrder(order);
        await _repositoryManager.SaveAsync();
        _loggerManager.LogInfo($"[{CompleteOrderAsync}] Successfully updated the order with ID {orderId} to status 'Completed'.");
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
            var shipmentPrice= await CalculatePrice(order,trackChanges);
        var cartItemViewModels = _mapper.Map<IEnumerable<CartItemViewModel>>(cartItems);
        return new OrderConfirmationViewModel(
      Id: order.Id,
      ShipmentPrice: shipmentPrice,
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
                  .Include(p => p.Product)
                  .ThenInclude(ph => ph.ProductPhotos)
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
        _loggerManager.LogInfo($"[{SendOrderAsync}] Successfully send order with ID {order.Id}");
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
        _loggerManager.LogInfo($"[{SendOrderAsync}] Successfully cancel order with ID {order.Id}");
        return true;
    }
    public async Task<IEnumerable<string>> GetCitiesAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Enumerable.Empty<string>();

        var cities = await _repositoryManager.CityRepository
            .FindCitiesByCondition(c => c.Name.StartsWith(searchTerm), trackChanges: true)
             .Select(c => c.Name)
            .Take(10) 
            .ToListAsync();

        return cities;
    }
    public async Task<IEnumerable<string>> GetEcontOfficesAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return Enumerable.Empty<string>();

        // Fetch the offices based on the HubName prefix (i.e., city name)
        var offices = await _repositoryManager.OfficeRepository
     .FindOfficesByCondition(o =>
         o.Address != null &&
         o.Address.City != null &&
         o.Address.City.Name.ToLower() == cityName.ToLower(),
         trackChanges: true)
     .Select(o => o.Address.FullAddress)
     .ToListAsync();

        return offices;
    }

    public async Task<JObject> GetAddressByLatAndLongAsync(double latitude, double longitude)
    {
        const string NominatimUrl = "https://nominatim.openstreetmap.org/reverse";
        var formattedLatitude = latitude.ToString(CultureInfo.InvariantCulture);
        var formattedLongitude = longitude.ToString(CultureInfo.InvariantCulture);
        using (var httpClient = new HttpClient())
        {
            var requestUrl = $"{NominatimUrl}?lat={formattedLatitude}&lon={formattedLongitude}&format=json";

            httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName/1.0");

            var response = await httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch geocoding data.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JObject.Parse(jsonResponse); 
        }
    }
    public async Task<decimal> CalculatePrice(Order order, bool trackChanges)
    {
        var cartItems = await _repositoryManager.CartItemRepository
           .FindCartItemsByConditionAsync(c => c.UserId == order.UserId, trackChanges)
           .Include(p => p.Product)
           .ThenInclude(ph => ph.ProductPhotos)
           .Include(u => u.User)
           .ToListAsync();
        var cartItemViewModels = _mapper.Map<IEnumerable<CartItemViewModel>>(cartItems);
        decimal sum = 0m;
        try { 
        foreach (var CartItem in cartItemViewModels)
        {
            var currProduct = _repositoryManager.ProductRepository
                .FindProductByConditionAsync(p => p.Id == CartItem.ProductId, trackChanges)
                .FirstOrDefault();
                var senderUserId = _repositoryManager.FarmerRepository
                    .FindFarmersByConditionAsync(f => f.Id == currProduct.FarmerId, trackChanges: trackChanges)
                    .FirstOrDefault().UserId;
                var senderUser = _repositoryManager.UserRepository
                    .FindUsersByConditionAsync(u=>u.Id == senderUserId, trackChanges: trackChanges) .FirstOrDefault();

                var currUser = _repositoryManager.UserRepository.FindUsersByConditionAsync(u => u.Id == order.UserId, trackChanges: trackChanges).FirstOrDefault();
            var FarmerLocation = _repositoryManager.FarmerLocationRepository
                .FindFarmerLocationsByConditionAsync(f => f.Farmer.UserId == senderUser.Id, trackChanges: trackChanges)
                .FirstOrDefault();
            var locationLat = FarmerLocation.Latitude;
            var locationLon = FarmerLocation.Longitude;
            var address = await GetAddressByLatAndLongAsync(locationLon, locationLat);
            var cityDtoReveiver = new CityDTO
            {

                Name = order.City,
                Country = new CountryDTO { Code3 = "BGR" }
            };
            var cityDtoSender = new CityDTO
            {

                Name = address["address"]?["city"]?.ToString(),
                Country = new CountryDTO { Code3 = "BGR" }
            };
                var shipmentRequest = new CalculateShipmentPriceRequest(
                    new ShippingLabelDTO(
                        senderClient: new ClientProfileDTO($"{senderUser.FirstName} {senderUser.LastName}", new List<string> { senderUser.PhoneNumber }),
                      senderAddress: new AddressDTO(
                            city: cityDtoSender,
                            streetName: address["address"]?["road"]?.ToString(),
                            streetNum: address["address"]?["num"]?.ToString()
                            ),
                        receiverClient: new ClientProfileDTO($"{currUser.FirstName} {currUser.LastName}", new List<string> { order.PhoneNumber }),
                        receiverAddress: new AddressDTO(
                            city: cityDtoReveiver,
                            streetName: order.StreetName,
                            streetNum: order.StreetNum
                        ),
                        packCount: CartItem.Quantity,
                        weigth: 1 * (order.OrderProducts.Count()),
                        shipmentType: ShipmentType.Pack,
                        shipmentDescription: "Order shipment",
                        orderPrice: (double)CartItem.TotalPrice
                    )
                );
                var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var httpClient = new HttpClient();
                var _econtLabelService = new EcontLabelService(configuration, httpClient);
                var TotalPriceTask = _econtLabelService.CalculateShipmentAsync(shipmentRequest);

                double? TotalPriceNullable = await TotalPriceTask;
                sum += TotalPriceNullable.HasValue ? (decimal)TotalPriceNullable.Value : 0m;
            }
}
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new Exception();
        }
        return sum;
    }
}
