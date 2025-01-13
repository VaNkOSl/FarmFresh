
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Extensions;

public class OrderStatusUpdater : IHostedService, IDisposable
{
    private Timer timer;
    private readonly IServiceProvider _serviceProvider;

    public OrderStatusUpdater(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private async void UpdateOrderStatuses(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();

            var shippedOrders = await repository.OrderRepository
                .FindAllOrders(trackChanges:true)
                .Where(o => o.OrderStatus == OrderStatus.Shipped && o.ShippedDate.Value.AddSeconds(2) <= DateTime.UtcNow)
                .ToListAsync();

            foreach (var order in shippedOrders)
            {
                order.OrderStatus = OrderStatus.ReadyForPickup;
                repository.OrderRepository.UpdateOrder(order);
            }

            await repository.SaveAsync();
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer(UpdateOrderStatuses, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}
