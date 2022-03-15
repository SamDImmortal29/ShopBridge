using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopbridgeWebAPI.Data.Repository;
using ShopbridgeWebAPI.Domain.Dtos;
using ShopbridgeWebAPI.Domain.Models;
using ShopbridgeWebAPI.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopbridgeWebAPI.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        public ProductService(ILogger<ProductService> logger,
                              IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetAllProducts(int pageSize, int page)
        {
            try
            {
                if (pageSize <= 0)
                    pageSize = 50;
                if (page < 1)
                    page = 1;

                var products = _productRepository.GetAsNoTracking();
                var paginatedProducts = await products
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

                var paginatedProductDtoList = TransformObject<List<ProductDto>, List<Product>>(paginatedProducts);
                return paginatedProductDtoList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching all product information: {ex}");
                throw;
            }
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            try
            {
                var product = await _productRepository.FindAsync(id);
                if (product == null)
                    throw new KeyNotFoundException($"Product with given id: {id} doesn't exists in the store.");
                var productDto = TransformObject<ProductDto, Product>(product);
                return productDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"error while geting item product information: {ex}");
                throw;
            }
        }

        public async Task<ProductDto> AddProduct(ProductDto productDto)
        {
            try
            {
                await CheckIfExists(productDto);
                var product = TransformObject<Product, ProductDto>(productDto);
                product = await _productRepository.AddAsync(product);
                productDto.Product_Id = product.Product_Id;
                return productDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while creating product: {ex}");
                throw;
            }
        }

        public async Task UpdateProduct(ProductDto productDto)
        {
            try
            {

                await CheckIfExists(productDto);
                var product = await _productRepository.FindAsync(productDto.Product_Id);
                if (product == null)
                    throw new KeyNotFoundException($"Product doesn't exists in the store.");
                AssignProperties(productDto, product);
                await _productRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating product: {ex}");
                throw;
            }
        }

        public async Task DeleteProduct(int id)
        {
            try
            {
                var isDeleted = await _productRepository.DeleteAsync(id);
                if (!isDeleted)
                    throw new KeyNotFoundException($"Product doesn't exists in the store.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting product : {ex}");
                throw;
            }
        }

        private void AssignProperties(ProductDto productDto, Product product)
        {
            //Note:A generic util can be made that automatically assigns all the properties from one object to another
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Quantity = productDto.Quantity;
        }
        private async Task CheckIfExists(ProductDto productDto)
        {
            var existingProduct = await _productRepository.GetByNameAsync(productDto.Name);
            if (existingProduct != null && !productDto.Product_Id.Equals(existingProduct.Product_Id))
                throw new ApplicationException($"Product with name : {productDto.Name} already exists in the store.");
        }

        private T TransformObject<T, U>(U sourceObj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(sourceObj));
        }

    }
}
