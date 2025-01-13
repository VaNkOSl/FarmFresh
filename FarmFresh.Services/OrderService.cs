using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.OrdersInterfaces;
using FarmFresh.Services.Econt;
using FarmFresh.Services.Helpers;
using FarmFresh.Services.Orders;
using FarmFresh.ViewModels.Order;
using LoggerService.Contacts;
using Newtonsoft.Json.Linq;

namespace FarmFresh.Services;

internal class OrderService : IOrderService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;
    private readonly IOrderManagmentService _orderManagmentService;
    private readonly IEcontManagmentService _econtManagmentService;

    public OrderService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper,
						IOrderManagmentService orderManagmentService,
						IEcontManagmentService econtManagmentService)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
        _orderManagmentService = orderManagmentService;
        _econtManagmentService = econtManagmentService;
    }

    public async Task<Guid> CheckoutAsync(CreateOrderDto model, Guid userId, bool trackChanges)
    {
       return await _orderManagmentService.CheckoutAsync(model, userId, trackChanges);
    }

    public async Task CompleteOrderAsync(Guid orderId, bool trackChanges) =>
        await _orderManagmentService.CompleteOrderAsync(orderId, trackChanges);

    public async Task CompleteOrderAsync(Guid orderId, bool trackChanges, PaymentOption paymentOption) =>
        await _orderManagmentService.CompleteOrderAsync(orderId, trackChanges, paymentOption);

    public async Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId, bool trackChanges) =>
        await _orderManagmentService.GetOrderConfirmationViewModelAsync(orderId, trackChanges);

    public async Task<List<OrderListViewModel>> GetOrdersForUserAsync(string userId, bool trackChanges) =>
       await _orderManagmentService.GetOrdersForUserAsync(userId, trackChanges);

    public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(Guid id, bool trackChanges)
    {
        var orderProduct = await OrderProductHelper.GetOrderProductAndCheckIfItExists(id, trackChanges, _repositoryManager, _loggerManager);

        return _mapper.Map<OrderDetailsViewModel>(orderProduct);
    }

    public async Task<IEnumerable<FarmerOrderListViewModel>> GetOrderConfirmationForFarmersViewModelAsync(Guid farmerId, bool trackChanges) =>
        await _orderManagmentService.GetOrderConfirmationForFarmersViewModelAsync(farmerId, trackChanges);

    public async Task<bool> SendOrderAsync(Guid orderId, bool trackChanges) =>
        await _orderManagmentService.SendOrderAsync(orderId, trackChanges);

    public async Task<bool> CancelOrder(Guid orderId, bool trackChanges) =>
        await _orderManagmentService.CancelOrder(orderId, trackChanges);

    public async Task<IEnumerable<string>> GetCitiesAsync(string searchTerm) =>
        await _econtManagmentService.GetCitiesAsync(searchTerm);

    public async Task<IEnumerable<string>> GetEcontOfficesAsync(string cityName) =>
        await _econtManagmentService.GetEcontOfficesAsync(cityName);

    public async Task<JObject> GetAddressByLatAndLongAsync(double latitude, double longitude) =>
      await _econtManagmentService.GetAddressByLatAndLongAsync(latitude, longitude);

    public async Task<decimal> CalculatePrice(Order order, bool trackChanges) =>
        await _econtManagmentService.CalculatePrice(order, trackChanges);

    public async Task<CreateLabelResponse> CreateLabel(Order order, bool trackChanges) =>
        await _econtManagmentService.CreateLabel(order, trackChanges);


}
