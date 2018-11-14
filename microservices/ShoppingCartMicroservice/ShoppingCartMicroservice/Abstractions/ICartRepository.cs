using System.Threading.Tasks;
using Techniqly.Microservices.Models;

namespace Techniqly.Microservices.Abstractions
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string cartId);
        Task AddItemsAsync(string cartId, string[] productIds);
        Task RemoveItemsAsync(string cartId, string[] productIds);
    }
}
