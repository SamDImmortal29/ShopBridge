using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShopbridgeWebAPI.Data.Repository;
using ShopbridgeWebAPI.Domain.Dtos;
using ShopbridgeWebAPI.Domain.Models;
using ShopbridgeWebAPI.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopbridgeUnitTest
{
    [TestFixture]
    public class ProductServiceUnitTest
    {
        private Mock<ILogger<ProductService>> _logger;
        private Mock<IProductRepository> _productRepository;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ProductService>>();
            _productRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_logger.Object, _productRepository.Object);
        }

        [Test]
        public async Task TestGetProduct_ReturnsProduct()
        {
            // Arrange
            var fakeProducts = GetFakeProducts();
            var existingProductId = 1;
            var foundProduct = fakeProducts.Where(x => x.Product_Id == existingProductId).FirstOrDefault();
            _productRepository.Setup(x => x.FindAsync(existingProductId)).Returns(Task.FromResult(foundProduct));


            // Act
            var product = await _productService.GetProduct(1);

            // Assert
            Assert.IsNotNull(product, "Result is null");
            Assert.AreEqual(1, product.Product_Id);
        }

        [Test]
        public void TestGetProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var notFoundProductId = 8;
            var fakeProducts = GetFakeProducts();
            var foundProduct = fakeProducts.Where(x => x.Product_Id == notFoundProductId).FirstOrDefault();
            _productRepository.Setup(x => x.FindAsync(notFoundProductId)).Returns(Task.FromResult(foundProduct));


            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.GetProduct(notFoundProductId));

            // Assert
            Assert.AreEqual("Product with given id: 8 doesn't exists in the store.", ex.Message);
        }

        [Test]
        public async Task TestAddProduct()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Description = "My Desc",
                Price = 35,
                Quantity = 10

            };
            var newProductWithId = new Product
            {
                Product_Id = 6,
                Name = "Test Product",
                Description = "My Desc",
                Price = 35,
                Quantity = 10

            };
            _productRepository.Setup(x => x.AddAsync(It.IsAny<Product>())).Returns(Task.FromResult(newProductWithId));

            // Act
            var addedProduct = await _productService.AddProduct(productDto);

            // Assert
            Assert.IsNotNull(addedProduct, "Result is null");
            Assert.AreEqual(6, addedProduct.Product_Id);
        }

        [Test]
        public void TestAddProduct_ThrowsExistingException()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Description = "My Desc",
                Price = 35,
                Quantity = 10

            };
            var existingProduct = new Product
            {
                Product_Id = 6,
                Name = "Test Product",
                Description = "My Desc",
                Price = 35,
                Quantity = 10

            };
            _productRepository.Setup(x => x.GetByNameAsync("Test Product")).Returns(Task.FromResult(existingProduct));

            // Act
            var ex = Assert.ThrowsAsync<ApplicationException>(() => _productService.AddProduct(productDto));

            // Assert
            Assert.AreEqual($"Product with name : {existingProduct.Name} already exists in the store.", ex.Message);
        }

        [Test]
        public void TestUpdateProduct_ThrowsExistingException()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Product_Id = 7,
                Name = "Test Product",
                Description = "My Desc2",
                Price = 25,
                Quantity = 5

            };
            var existingProduct = new Product
            {
                Product_Id = 6,
                Name = "Test Product",
                Description = "My Desc",
                Price = 35,
                Quantity = 10

            };
            _productRepository.Setup(x => x.GetByNameAsync("Test Product")).Returns(Task.FromResult(existingProduct));

            // Act
            var ex = Assert.ThrowsAsync<ApplicationException>(() => _productService.UpdateProduct(productDto));

            // Assert
            Assert.AreEqual($"Product with name : {existingProduct.Name} already exists in the store.", ex.Message);
        }

        [Test]
        public void TestUpdateProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Product_Id = 8,
                Name = "Test Product",
                Description = "My Desc2",
                Price = 25,
                Quantity = 5

            };
            var fakeProducts = GetFakeProducts();
            var foundProduct = fakeProducts.Where(x => x.Product_Id == productDto.Product_Id).FirstOrDefault();
            _productRepository.Setup(x => x.FindAsync(productDto.Product_Id)).Returns(Task.FromResult(foundProduct));

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.UpdateProduct(productDto));

            // Assert
            Assert.AreEqual("Product doesn't exists in the store.", ex.Message);
        }
        [Test]
        public void TestDeleteProduct_ThrowsKeyNotFoundException()
        {
            // Arrange
            var productId = 8;
            _productRepository.Setup(x => x.DeleteAsync(productId)).Returns(Task.FromResult(false));

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.DeleteProduct(productId));

            // Assert
            Assert.AreEqual("Product doesn't exists in the store.", ex.Message);
        }

        private static List<Product> GetFakeProducts()
        {
            var fakeProducts = new List<Product>
            {
                    new Product {Product_Id=1,Name="Product1",Description="TestDesc1",Price=10,Quantity=15 },
                    new Product {Product_Id=2,Name="Product2",Description="TestDesc2",Price=20,Quantity=25 },
                    new Product {Product_Id=3,Name="Product3",Description="TestDesc3",Price=30,Quantity=35 },
                    new Product {Product_Id=4,Name="Product4",Description="TestDesc4",Price=40,Quantity=45 },
            }.ToList();
            return fakeProducts;
        }
    }
}