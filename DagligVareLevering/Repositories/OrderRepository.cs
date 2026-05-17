using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<Order?> GetLatestUserOrderAsync(int userId)
        {
            return await QueryAsync()
             .Include(o => o.User)
             .Include(o => o.OrderLines)
             .ThenInclude(ol => ol.Product)
             .Where(o => o.UserId == userId)
             .OrderByDescending(o => o.TimeOfOrder)
             .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByWorkerAsync(int workerId)
        {
            return await QueryAsync()
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.Product)
            .Include(o => o.User)
            .Where(o => o.WorkerId == workerId && o.Status == OrderStatus.OutForDelivery)
            .ToListAsync();
        }

        public async Task<Order?> GetWorkerActiveOrderAsync(int workerId, int id)
        {
            return await QueryAsync()
                .Include(o => o.OrderLines.OrderBy(ol => ol.Product.StoreId))
                .ThenInclude(ol => ol.Product)
                .ThenInclude(p => p.Store)
                .Include(o => o.User)
                .Where(o => o.WorkerId == workerId && o.Status == OrderStatus.OutForDelivery && o.OrderId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersWithOrderLinesAndProducts(int userId)
        {
            return await QueryAsync()
                    .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                    .Where(o => o.UserId == userId)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersWithOrderLinesAndProducts()
        {
            return await QueryAsync()
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersWithNoWorker()
        {
            return await QueryAsync()
                    .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                    .Include(o => o.User)
                    .Where(o => o.WorkerId == null && (o.Status == OrderStatus.Processing || o.Status == OrderStatus.Received))
                    .ToListAsync();
        }

    }
}
