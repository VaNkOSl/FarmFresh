using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data
{
    public static class DBTransactionHelper
    {
        public static async Task ExecuteTransactionAsync(FarmFreshDbContext context, Func<Task> operations)
        {
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
