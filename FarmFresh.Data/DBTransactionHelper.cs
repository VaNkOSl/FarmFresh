using Microsoft.Extensions.DependencyInjection;

namespace FarmFresh.Data
{
    public static class DBTransactionHelper
    {
        public static async Task ExecuteTransactionAsync(IServiceProvider serviceProvider, Func<Task> operations)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FarmFreshDbContext>();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                await operations();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
