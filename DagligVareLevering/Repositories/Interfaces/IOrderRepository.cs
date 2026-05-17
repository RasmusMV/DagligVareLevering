using DagligVareLevering.Models;

namespace DagligVareLevering.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetWorkerActiveOrderAsync(int workerId, int id);
        Task<IEnumerable<Order>> GetOrdersByWorkerAsync(int workerId);
        Task<Order?> GetLatestUserOrderAsync(int userId);
        Task<IEnumerable<Order>> GetUserOrdersWithOrderLinesAndProducts(int userId);
        Task<IEnumerable<Order>> GetAllOrdersWithOrderLinesAndProducts();
        Task<IEnumerable<Order>> GetAllOrdersWithNoWorker();
    }
}
