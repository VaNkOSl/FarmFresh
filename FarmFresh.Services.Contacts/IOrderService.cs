using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data.Models;

namespace FarmFresh.Services.Contacts
{
    public interface IOrderService
    {
        Task<Guid> CheckoutAsync(Order order, Guid orderProductId, Guid userId);
    }
}
