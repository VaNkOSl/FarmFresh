using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Econt.APIServices;
using FarmFresh.ViewModels.Order;
using LoggerService.Contacts;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace FarmFresh.Services.Econt;

public class EcontManagmentService : IEcontManagmentService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public EcontManagmentService(IRepositoryManager repositoryManager,
                      ILoggerManager loggerManager,
                      IMapper mapper
                      )
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task<CreateLabelResponse> CreateLabel(Order order, bool trackChanges)
    {
        var currUser = await GetUserAsync(order.UserId, trackChanges);
        var currentAdress = await GetOrderAddressAsync(currUser.Id, trackChanges);
        var orderProduct = await GetOrderProductAsync(order.Id, trackChanges);
        var product = await GetProductAsync(orderProduct.ProductId, trackChanges);
        var farmer = await GetFarmerAsync(product.FarmerId, trackChanges);
        var senderUser = await GetUserAsync(farmer.UserId, trackChanges);
        var farmerLocation = await GetFarmerLocationAsync(senderUser.Id, trackChanges);

        var productCount = await GetProductCountAsync(order.Id, trackChanges);
        var totalPrice = await GetTotalPriceAsync(order.Id, trackChanges);

        var address = await GetAddressByLatAndLongAsync(farmerLocation.Longitude, farmerLocation.Latitude);
        var senderCityName = GetCityNameFromAddress(address);

        var senderAddress = CreateSenderAddress(senderCityName, address);
        var receiverAddress = CreateReceiverAddress(order.City, currentAdress);

        var shippingLabel = CreateShippingLabel(senderUser, currUser, senderAddress, receiverAddress, productCount, totalPrice);


        return await CreateEcontLabelAsync(shippingLabel);
    }

    private async Task<ApplicationUser?> GetUserAsync(Guid userId, bool trackChanges) =>
        await _repositoryManager.UserRepository
        .FindUsersByConditionAsync(u => u.Id == userId, trackChanges)
        .FirstOrDefaultAsync();

    private async Task<Order?> GetOrderAddressAsync(Guid userId, bool trackChanges) =>
        await _repositoryManager.OrderRepository
        .FindOrderByConditionAsync(o => o.UserId == userId, trackChanges)
        .FirstOrDefaultAsync();

    private async Task<OrderProduct?> GetOrderProductAsync(Guid orderId, bool trackChanges) =>
        await _repositoryManager.OrderProductRepository
        .FindOrderProductByConditionAsync(op => op.OrderId == orderId, trackChanges)
        .FirstOrDefaultAsync();

    private async Task<Product?> GetProductAsync(Guid productId, bool trackChanges) =>
        await _repositoryManager.ProductRepository
        .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
        .FirstOrDefaultAsync();

    private async Task<Farmer?> GetFarmerAsync(Guid farmerId, bool trackChanges) =>
        await _repositoryManager.FarmerRepository
        .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
        .FirstOrDefaultAsync();

    private async Task<FarmerLocation?> GetFarmerLocationAsync(Guid userId, bool trackChanges) =>
        await _repositoryManager.FarmerLocationRepository
        .FindFarmerLocationsByConditionAsync(f => f.Farmer.UserId == userId, trackChanges)
        .FirstOrDefaultAsync();

    private async Task<int> GetProductCountAsync(Guid orderId, bool trackChanges) =>
        await _repositoryManager.OrderProductRepository
        .FindOrderProductByConditionAsync(o => o.OrderId == orderId, trackChanges)
        .CountAsync();

    private async Task<decimal> GetTotalPriceAsync(Guid orderId, bool trackChanges) =>
        await _repositoryManager.OrderProductRepository
        .FindOrderProductByConditionAsync(o => o.OrderId == orderId, trackChanges)
        .Select(o => o.Price * o.Quantity)
        .SumAsync();

    private string GetCityNameFromAddress(JObject address)
    {
        var addressComponents = address["results"]?[0]?["address_components"];
        return addressComponents?
            .FirstOrDefault(component => component["types"]?.Any(type => type.ToString() == "locality") ?? false)?
            ["long_name"]?.ToString()?.Trim();
    }

    private AddressDTO CreateSenderAddress(string cityName, JObject address)
    {
        var streetName = address["results"]?[0]?["address_components"]?
            .FirstOrDefault(component => component["types"]?.Any(type => type.ToString() == "route") ?? false)?
            ["long_name"]?.ToString()?.Trim();

        var cityDtoSender = new CityDTO
        {
            Name = cityName,
            Country = new CountryDTO { Code3 = "BGR" }
        };

        return new AddressDTO(cityDtoSender, streetName, "3");
    }

    private AddressDTO CreateReceiverAddress(string cityName, Order currentAdress)
    {
        var cityDtoReceiver = new CityDTO
        {
            Name = cityName,
            Country = new CountryDTO { Code3 = "BGR" }
        };

        return new AddressDTO(cityDtoReceiver, currentAdress.StreetName, currentAdress.StreetNum);
    }

    private ShippingLabelDTO CreateShippingLabel(ApplicationUser senderUser, ApplicationUser currUser, AddressDTO senderAddress, AddressDTO receiverAddress, int productCount, decimal totalPrice)
    {
        return new ShippingLabelDTO(
            new ClientProfileDTO(senderUser.FirstName + " " + senderUser.LastName, new List<string> { "0000000000" }),
            senderAddress,
            new ClientProfileDTO(currUser.FirstName + " " + currUser.LastName, new List<string> { "1111111111" }),
            receiverAddress,
            productCount,
            productCount * 1,
            ShipmentType.Pack,
            "Order shipment",
            (double)totalPrice
        );
    }

    private async Task<CreateLabelResponse> CreateEcontLabelAsync(ShippingLabelDTO shippingLabel)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var httpClient = new HttpClient();
        var _econtLabelService = new EcontLabelService(configuration, httpClient);
        var LabelRequest = new CreateLabelRequest(shippingLabel);

        return await _econtLabelService.CreateLabelAsync(LabelRequest);
    }

    public async Task<JObject> GetAddressByLatAndLongAsync(double latitude, double longitude)
    {
        var apiKey = "AIzaSyDn87dIETpeaJSov9jznA0k9YUAs7Fs5QA";
        var requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={longitude},{latitude}&key={apiKey}";

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch geocoding data.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Google Maps Response: {jsonResponse}");
            return JObject.Parse(jsonResponse);
        }
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

        try
        {

            foreach (var CartItem in cartItemViewModels)
            {
                var currProduct = await _repositoryManager.ProductRepository
                    .FindProductByConditionAsync(p => p.Id == CartItem.ProductId, trackChanges)
                    .FirstOrDefaultAsync();

                var senderUserId = await _repositoryManager.FarmerRepository
                    .FindFarmersByConditionAsync(f => f.Id == currProduct.FarmerId, trackChanges: trackChanges)
                    .FirstOrDefaultAsync();

                var senderUser = await _repositoryManager.UserRepository
                    .FindUsersByConditionAsync(u => u.Id == senderUserId.UserId, trackChanges: trackChanges)
                    .FirstOrDefaultAsync();

                var currUser = await _repositoryManager.UserRepository
                    .FindUsersByConditionAsync(u => u.Id == order.UserId, trackChanges: trackChanges)
                    .FirstOrDefaultAsync();

                var FarmerLocation = await _repositoryManager.FarmerLocationRepository
                    .FindFarmerLocationsByConditionAsync(f => f.Farmer.UserId == senderUser.Id, trackChanges: trackChanges)
                    .FirstOrDefaultAsync();

                var locationLat = FarmerLocation.Latitude;
                var locationLon = FarmerLocation.Longitude;
                var address = await GetAddressByLatAndLongAsync(locationLon, locationLat);

                var cityDtoReveiver = new CityDTO
                {

                    Name = order.City,
                    Country = new CountryDTO { Code3 = "BGR" }
                };

                var addressComponents = address["results"]?[0]?["address_components"];
                var senderCityName = addressComponents?
                    .FirstOrDefault(component => component["types"]?.Any(type => type.ToString() == "locality") ?? false)?
                    ["long_name"]?.ToString()?.Trim();

                var cityDtoSender = new CityDTO
                {

                    Name = senderCityName,
                    Country = new CountryDTO { Code3 = "BGR" }
                };

                var streetName = addressComponents?
                .FirstOrDefault(component => component["types"]?.Any(type => type.ToString() == "route") ?? false)?
                ["long_name"]?.ToString()?.Trim();

                var shipmentRequest = new CalculateShipmentPriceRequest(
                new ShippingLabelDTO(
                    senderClient: new ClientProfileDTO($"{senderUser.FirstName} {senderUser.LastName}", new List<string> { senderUser.PhoneNumber }),
                    senderAddress: new AddressDTO(
                        city: cityDtoSender,
                        streetName: streetName,
                        streetNum: "3"
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
                var TotalPriceTask = await _econtLabelService.CalculateShipmentAsync(shipmentRequest);

                double? totalPriceNullable = TotalPriceTask;
                sum += totalPriceNullable.HasValue ? (decimal)totalPriceNullable.Value : 0m;
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
