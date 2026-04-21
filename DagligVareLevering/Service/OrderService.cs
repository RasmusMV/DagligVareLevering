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

        public Task<IEnumerable<Order>> SortById()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> SortByIdDescending()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> SortByTotalPrice()
        {
            throw new NotImplementedException();
        }
    }
}
