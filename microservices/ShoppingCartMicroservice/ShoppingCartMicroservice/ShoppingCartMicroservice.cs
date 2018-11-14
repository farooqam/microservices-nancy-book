using System.Threading.Tasks;
using AzureFunctions.Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Techniqly.Microservices.Abstractions;
using Techniqly.Microservices.Models;

namespace Techniqly.Microservices
{
    [DependencyInjectionConfig(typeof(DependencyConfig))]
    public static class ShoppingCartMicroservice
    {
        [FunctionName("GetCart")]
        public static async Task<IActionResult> GetCart(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "cart/{cartId}")] HttpRequest req,
            string cartId,
            [Inject] ICartRepository repository,
            ILogger log)
        {
            var cart = await repository.GetCartAsync(cartId);

            if(cart is NotFoundCart) return new NotFoundResult();

            return new OkObjectResult(cart);
        }

        [FunctionName("AddItems")]
        public static async Task<IActionResult> AddItems(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "cart/{cartId}")]
            HttpRequest req,
            string cartId,
            [Inject] IRequestConverter<string[]> requestConverter,
            [Inject] ICartRepository repository,
            ILogger log)
        {
            var productIds = await requestConverter.ConvertAsync(req);
            await repository.AddItemsAsync(cartId, productIds);
            return new AcceptedResult();
        }

        [FunctionName("RemoveItems")]
        public static async Task<IActionResult> RemoveItems(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "cart/{cartId}")]
            HttpRequest req,
            string cartId,
            [Inject] IRequestConverter<string[]> requestConverter,
            [Inject] ICartRepository repository,
            ILogger log)
        {
            var productIds = await requestConverter.ConvertAsync(req);
            await repository.RemoveItemsAsync(cartId, productIds);
            return new NoContentResult();
        }
    }
}
