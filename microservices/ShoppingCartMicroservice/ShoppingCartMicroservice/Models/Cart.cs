using System.Collections.Generic;
using System.Linq;

namespace Techniqly.Microservices.Models
{
    public class Cart
    {
        private readonly List<Product> _products = new List<Product>();
        
        public string Id { get; set; }

        public IEnumerable<Product> Items => _products.AsEnumerable();

        public void AddItem(Product product)
        {
            _products.Add(product);
        }

        public void RemoveItem(string productId)
        {
            _products.RemoveAll(p => p.Id == productId);
        }
        
    }

    public class NotFoundCart : Cart
    {

    }
}
