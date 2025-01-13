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
                      IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task<CreateLabelResponse> CreateLabel(Order order, bool trackChanges)
    {
        var orderDetails = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(o => o.Id == order.Id, trackChanges)
            .Include(u => u.User)
            .Include(op => op.OrderProducts)
            .ThenInclude(op => op.Product)
            .ThenInclude(p => p.Farmer.User)
            .FirstOrDefaultAsync();

        var farmerLocation = await _repositoryManager.FarmerLocationRepository
           .FindFarmerLocationsByConditionAsync(fl => fl.Farmer.UserId == orderDetails.OrderProducts.First().Product.Farmer.UserId, trackChanges)
           .FirstOrDefaultAsync();

        var address = await GetAddressByLatAndLongAsync(farmerLocation.Longitude, farmerLocation.Latitude);
        var senderCityName = GetCityNameFromAddress(address);

        var senderAddress = CreateSenderAddress(senderCityName, address);
        var receiverAddress = CreateReceiverAddress(order.City, orderDetails);

        var productCount = orderDetails.OrderProducts.Sum(op => op.Quantity);
        var totalPrice = orderDetails.OrderProducts.Sum(op => op.Price * op.Quantity);

        var shippingLabel = CreateShippingLabel(orderDetails.OrderProducts.First().Product.Farmer.User, orderDetails.User, senderAddress, receiverAddress, productCount, totalPrice);

        return await CreateEcontLabelAsync(shippingLabel);
    }

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
            .ThenInclude(p => p.Farmer.User)
            .Include(ph => ph.Product.ProductPhotos)
            .ToListAsync();

        var farmerLocations = await _repositoryManager.FarmerLocationRepository
            .FindFarmerLocationsByConditionAsync(
            f => cartItems.Select(ci => ci.Product.Farmer.UserId).Contains(f.Farmer.UserId), trackChanges)
            .Include(f => f.Farmer)
            .ToListAsync();

        var cartItemViewModels = _mapper.Map<IEnumerable<CartItemViewModel>>(cartItems);
        decimal totalSum = 0m;

        try
        {
            foreach(var cartItem in cartItemViewModels)
            {
                var product = cartItems.First(ci => ci.ProductId == cartItem.ProductId).Product;
                var farmer = product.Farmer;
                var senderUser = farmer.User;
                var farmerLocation = farmerLocations.FirstOrDefault(fl => fl.Farmer.UserId == senderUser.Id);

                var address = await GetAddressByLatAndLongAsync(farmerLocation.Longitude, farmerLocation.Latitude);

                var senderCityName = ExtractCityNameFromAddress(address);
                var streetName = ExtractStreetNameFromAddress(address);

                var shipmentRequest = BuildShipmentRequest(cartItem, order, senderUser, senderCityName, streetName);
                var shipmentPrice = await CalculateShipmentPrice(shipmentRequest);
                totalSum += shipmentPrice;
            }
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw;
        }

        return totalSum;
    }

    private string ExtractCityNameFromAddress(JObject address)
    {
        var addressComponents = address["results"]?[0]?["address_components"];
        return addressComponents?
            .FirstOrDefault(component => component["types"]?.Any(type => type.ToString() == "locality") ?? false)?
            ["long_name"]?.ToString()?.Trim();
    }

    private string ExtractStreetNameFromAddress(JObject address)
    {
        var addressComponents = address["results"]?[0]?["address_components"];
        return addressComponents?
            .FirstOrDefault(component => component["types"]?.Any(type => type.ToString() == "route") ?? false)?
            ["long_name"]?.ToString()?.Trim();
    }

    private CalculateShipmentPriceRequest BuildShipmentRequest(CartItemViewModel cartItem, Order order,
                                                               ApplicationUser senderUser, string senderCityName,
                                                               string streetName)
    {
        var senderCity = new CityDTO
        {
            Name = senderCityName,
            Country = new CountryDTO { Code3 = "BGR" }
        };

        var receiverCity = new CityDTO
        {
            Name = order.City,
            Country = new CountryDTO { Code3 = "BGR" }
        };

        return new CalculateShipmentPriceRequest(
       new ShippingLabelDTO(
           senderClient: new ClientProfileDTO($"{senderUser.FirstName} {senderUser.LastName}", new List<string> { senderUser.PhoneNumber }),
           senderAddress: new AddressDTO(
               city: senderCity,
               streetName: streetName,
               streetNum: "3"
           ),
           receiverClient: new ClientProfileDTO($"{order.FirstName} {order.LastName}", new List<string> { order.PhoneNumber }),
           receiverAddress: new AddressDTO(
               city: receiverCity,
               streetName: order.StreetName,
               streetNum: order.StreetNum
           ),
           packCount: cartItem.Quantity,
           weigth: 1 * order.OrderProducts.Count(),
           shipmentType: ShipmentType.Pack,
           shipmentDescription: "Order shipment",
           orderPrice: (double)cartItem.TotalPrice
       )
   );
    }

    private async Task<decimal> CalculateShipmentPrice(CalculateShipmentPriceRequest request)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var httpClient = new HttpClient();
        var econtLabelService = new EcontLabelService(configuration, httpClient);
        var shipmentPrice = await econtLabelService.CalculateShipmentAsync(request);
        return shipmentPrice.HasValue ? (decimal)shipmentPrice.Value : 0m;
    }
}
