using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Techniqly.Microservices.Abstractions;
using Techniqly.Microservices.Models;

namespace Techniqly.Microservices
{
    public class InMemoryCartRepository : ICartRepository
    {
        private readonly ConcurrentBag<Cart> _carts = new ConcurrentBag<Cart>();

        public async Task<Cart> GetCartAsync(string cartId)
        {
            var cart = _carts.SingleOrDefault(c => c.Id == cartId);
            return cart ?? new NotFoundCart();
        }

        public async Task AddItemsAsync(string cartId, string[] productIds)
        {
            var cart = await GetCartAsync(cartId);

            if (cart is NotFoundCart)
            {
                cart = new Cart {Id = cartId};

                for (var i = 0; i < productIds.Length; i++)
                {
                    var product = new Product
                    {
                        Id = $"p{i + 1}",
                        Description = $"product #{i + 1}",
                        Name = $"p{i + 1}",
                        Price = new Price
                        {
                            Amount = 99.99m,
                            Currency = "USD"
                        }
                    };

                    cart.AddItem(product);
                }

                _carts.Add(cart);
            }
        }

        public async Task RemoveItemsAsync(string cartId, string[] productIds)
        {
            var cart = await GetCartAsync(cartId);

            if(cart is NotFoundCart) return;

            foreach (var productId in productIds)
            {
                cart.RemoveItem(productId);
            }
        }
    }
}
