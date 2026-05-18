using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<List<Product>> GetAllProductsWithStoreAsync()
        {
            return await QueryAsync()
                .Include(p => p.Store)
                .ToListAsync();
        }

        public async Task<List<Product>> GetPopularProductsWithStoreAsync(int count)
        {
            return await QueryAsync()
                .Include(p => p.Store)
                .Take(count)
                .ToListAsync();
        }
    }
}
