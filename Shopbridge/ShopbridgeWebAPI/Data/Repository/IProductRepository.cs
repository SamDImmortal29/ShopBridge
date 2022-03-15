using ShopbridgeWebAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopbridgeWebAPI.Data.Repository
{
    public interface IProductRepository
    {
        IQueryable<Product> Get();
        IQueryable<Product> GetAsNoTracking();
        Task<Product> GetByNameAsync(string name);
        Task<Product> AddAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<Product> FindAsync(int id);
        Task SaveChangesAsync();
    }
}
