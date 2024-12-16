using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FarmFresh.Services.Contacts
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(Guid productId, string sessionKey, ISession session);
        void RemoveFromCart(Guid productId, string sessionKey, ISession session);
        Task<bool> UpdateCartQuantityAsync(Guid productId, int quantityChange, string sessionKey, ISession session);
    }
}
