using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Order;
using Microsoft.AspNetCore.Http;

namespace FarmFresh.Services
{
    public class CartService : ICartService
    {
        private readonly IProductRepository _productRepository;

        public CartService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<bool> AddToCartAsync(Guid productId, string sessionKey, ISession session)
        {
            var cart = session.Get<List<CartItemViewModel>>(sessionKey) ?? new List<CartItemViewModel>();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);

            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return false;

            int requestedQuantity = existingItem != null ? existingItem.Quantity + 1 : 1;
            if (product.StockQuantity < requestedQuantity) return false;

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Quantity = 1,
                    Price = product.Price
                });
            }

            session.Set(sessionKey, cart);
            return true;
        }

        public void RemoveFromCart(Guid productId, string sessionKey, ISession session)
        {
            var cart = session.Get<List<CartItemViewModel>>(sessionKey) ?? new List<CartItemViewModel>();
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                session.Set(sessionKey, cart);
            }
        }

        public async Task<bool> UpdateCartQuantityAsync(Guid productId, int quantityChange, string sessionKey, ISession session)
        {
            var cart = session.Get<List<CartItemViewModel>>(sessionKey) ?? new List<CartItemViewModel>();
            var itemInCart = cart.FirstOrDefault(c => c.ProductId == productId);

            if (itemInCart == null) return false;

            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return false;

            int newQuantity = itemInCart.Quantity + quantityChange;

            if (newQuantity > product.StockQuantity || newQuantity < 1) return false;

            itemInCart.Quantity = newQuantity;
            session.Set(sessionKey, cart);
            return true;
        }
    }
}
