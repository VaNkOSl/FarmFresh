using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
