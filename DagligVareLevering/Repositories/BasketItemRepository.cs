using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Repositories
{
    public class BasketItemRepository : GenericRepository<BasketItem>, IBasketItemRepository
    {
        public BasketItemRepository(AppDbContext context) : base(context) { }

        public async Task<List<BasketItem>> GetUserBasketItemsWithProductsAsync(int userId)
        {
            return await QueryAsync()
                .Include(b => b.Product)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
    }
}
