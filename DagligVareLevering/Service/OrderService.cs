using DagligVareLevering.Models;

namespace DagligVareLevering.Service
{
    public class OrderService : IOrderService
    {
        private readonly IService<Order> _dbService;

        public OrderService(IService<Order> dbService)
        {
            _dbService = dbService;
        }

        public async Task<IEnumerable<Order>> SortById()
        {
            return (await _dbService.GetObjectsAsync()).OrderBy(x => x.OrderId);
        }

        public async Task<IEnumerable<Order>> SortByIdDescending()
        {
            return (await _dbService.GetObjectsAsync()).OrderByDescending(x => x.OrderId);
        }

        public async Task<IEnumerable<Order>> SortByTotalPrice()
        {
            return (await _dbService.GetObjectsAsync()).OrderBy(x => x.GetTotalPrice());
        }
    }
}
