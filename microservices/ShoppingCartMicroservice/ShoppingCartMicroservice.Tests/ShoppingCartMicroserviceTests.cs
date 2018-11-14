using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Techniqly.Microservices.Abstractions;
using Techniqly.Microservices.Models;
using Xunit;

namespace Techniqly.Microservices.Tests
{
    public class ShoppingCartMicroserviceTests
    {
        [Fact]
        public async Task GivenShoppingCartId_ShouldGetShoppingCart()
        {
            // Arrange
            var expectedShoppingCart = new Cart {Id = "foo"};
            expectedShoppingCart.AddItem(new Product
            {
                Id = "p1",
                Name = "shirt",
                Description = "a nice shirt",
                Price = new Price
                {
                    Currency = "USD",
                    Amount = 39.99m
                }
            });

            expectedShoppingCart.AddItem(new Product
            {
                Id = "p2",
                Name = "jeans",
                Description = "a nice pair of jeans",
                Price = new Price
                {
                    Currency = "USD",
                    Amount = 59.99m
                }
            });

            var mockHttpRequest = new Mock<HttpRequest>();
            var mockLogger = new Mock<ILogger>();
            var mockRepository = new Mock<ICartRepository>();
            mockRepository.Setup(m => m.GetCartAsync(expectedShoppingCart.Id)).ReturnsAsync(expectedShoppingCart);

            // Act
            var result = (OkObjectResult)await ShoppingCartMicroservice.GetCart(mockHttpRequest.Object, expectedShoppingCart.Id, mockRepository.Object, mockLogger.Object);
            
            // Assert
            var actualShoppingCart = result.Value as Cart;
            actualShoppingCart.Should().BeEquivalentTo(expectedShoppingCart);
        }

        [Fact]
        public async Task GivenShoppingCartNotFound_ReturnsNotFoundHttpResult()
        {
            // Arrange
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockLogger = new Mock<ILogger>();
            var mockRepository = new Mock<ICartRepository>();
            mockRepository.Setup(m => m.GetCartAsync(It.IsAny<string>())).ReturnsAsync(new NotFoundCart());

            // Act
            var result = (NotFoundResult)await ShoppingCartMicroservice.GetCart(mockHttpRequest.Object, "foo", mockRepository.Object, mockLogger.Object);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CartItems_WhenAdded_AreAddedToRepository()
        {
            // Arrange
            var cartId = "foo";
            var productIds = new[] { "p1", "p2", "p3" };
            
            var mockLogger = new Mock<ILogger>();
            var mockHttpRequest = new Mock<HttpRequest>();
            
            var mockRepository = new Mock<ICartRepository>();

            var mockRequestConverter = new Mock<IRequestConverter<string[]>>();
            mockRequestConverter.Setup(m => m.ConvertAsync(mockHttpRequest.Object)).ReturnsAsync(productIds);

            // Act
            var result = (AcceptedResult)await ShoppingCartMicroservice.AddItems(mockHttpRequest.Object, cartId, mockRequestConverter.Object, mockRepository.Object, mockLogger.Object);

            // Assert
            result.Should().NotBeNull();
            mockRepository.Verify(m => m.AddItemsAsync(cartId, productIds), Times.Once);
        }

        [Fact]
        public async Task CartItems_WhenDeleted_AreDeletedFromRepository()
        {
            // Arrange
            var cartId = "foo";
            var productIds = new[] { "p1", "p2", "p3" };

            var mockLogger = new Mock<ILogger>();
            var mockHttpRequest = new Mock<HttpRequest>();

            var mockRepository = new Mock<ICartRepository>();

            var mockRequestConverter = new Mock<IRequestConverter<string[]>>();
            mockRequestConverter.Setup(m => m.ConvertAsync(mockHttpRequest.Object)).ReturnsAsync(productIds);

            // Act
            var result = (NoContentResult)await ShoppingCartMicroservice.RemoveItems(mockHttpRequest.Object, cartId, mockRequestConverter.Object, mockRepository.Object, mockLogger.Object);

            // Assert
            result.Should().NotBeNull();
            mockRepository.Verify(m => m.RemoveItemsAsync(cartId, productIds), Times.Once);
        }
    }
}
