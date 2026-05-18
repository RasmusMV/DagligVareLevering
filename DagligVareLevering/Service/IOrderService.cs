using DagligVareLevering.Models;

namespace DagligVareLevering.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> SortById();
        Task<IEnumerable<Order>> SortByIdDescending();
        Task<IEnumerable<Order>> SortByTotalPrice();
        Task<int> GetTotalOrders();
        Task<decimal> GetTotalRevenue();
        Task<int> GetMonthlyOrderCount();
        Task<decimal> GetMonthlyRevenue();
    }
}
