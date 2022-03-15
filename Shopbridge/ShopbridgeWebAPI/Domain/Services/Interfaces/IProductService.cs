using ShopbridgeWebAPI.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopbridgeWebAPI.Domain.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProducts(int pageSize, int page);
        Task<ProductDto> GetProduct(int id);
        Task<ProductDto> AddProduct(ProductDto productDto);
        Task UpdateProduct(ProductDto productDto);
        Task DeleteProduct(int id);
    }
}
