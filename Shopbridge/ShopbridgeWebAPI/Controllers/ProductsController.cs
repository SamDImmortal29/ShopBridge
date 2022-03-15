using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopbridgeWebAPI.Domain.Dtos;
using ShopbridgeWebAPI.Domain.Models;
using ShopbridgeWebAPI.Domain.Services.Interfaces;

namespace ShopbridgeWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(int pageSize = 50, int page = 1)
        {
            var productDtoList = await _productService.GetAllProducts(pageSize, page);
            return Ok(productDtoList);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var productDto = await _productService.GetProduct(id);
            return Ok(productDto);
        }


        [HttpPost]
        public async Task<ActionResult<ProductDto>> AddProduct(ProductDto productDto)
        {
            await _productService.AddProduct(productDto);
            return Ok(new
            {
                Data = productDto,
                Message = "Product Added Successfully"
            });
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            await _productService.UpdateProduct(productDto);
            return Ok(new { Message = "Product Updated Successfully" });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok(new { Message = "Product Deleted Successfully" });
        }
    }
}
