using Microsoft.EntityFrameworkCore;
using ShopbridgeWebAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopbridgeWebAPI.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Shopbridge_Context _dbContext;

        public ProductRepository(Shopbridge_Context dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Product> Get()
        {
            return _dbContext.Product;
        }

        public IQueryable<Product> GetAsNoTracking()
        {
            var query = _dbContext.Product
                .AsNoTracking();
            return query;
        }
        public async Task<Product> GetByNameAsync(string name)
        {
            var product = await _dbContext.Product
            .FirstOrDefaultAsync(u => u.Name == name);
            return product;
        }
        public async Task<Product> AddAsync(Product product)
        {
            await _dbContext.Product.AddAsync(product);
            await SaveChangesAsync();
            return product;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var productToBeDeleted = await FindAsync(id);
            if (productToBeDeleted != null)
            {
                _dbContext.Product.Remove(productToBeDeleted);
                await SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Product> FindAsync(int id)
        {
            var product = await _dbContext.Product.FindAsync(id);
            return product;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
