using DagligVareLevering.Models;

namespace DagligVareLevering.Service.Interfaces
{
    public interface IOrderService : IService<Order>
    {
        Task<Order> GetLatestUserOrderAsync(int userId);
        Task<Order?> GetWorkerActiveOrderAsync(int workerId, int id);
        Task<IEnumerable<Order>> GetOrdersByWorkerAsync(int workerId);
        Task MarkOrderAsDeliveredAsync(int orderId, int workerId);
        Task<IEnumerable<Order>> GetUserOrdersWithOrderLinesAndProducts(int userId);
        Task<IEnumerable<Order>> GetAllOrdersWithOrderLinesAndProducts();
        Task<IEnumerable<Order>> GetAllOrdersWithNoWorker();
        Task TakeOrderAsync(int orderId, int workerId);
        Task ConfirmOrderAsync(int userId, string? deliveryAdress, string paymentMethod);
        Task CancelOrderAsync(int orderId);
        Task CheckoutAsync(int userId, string userAddress, List<BasketItem> basketItems);
        Task<IEnumerable<Order>> SortById();
        Task<IEnumerable<Order>> SortByIdDescending();
        Task<IEnumerable<Order>> SortByTotalPrice();
        Task<int> GetTotalOrders();
        Task<decimal> GetTotalRevenue();
        Task<int> GetMonthlyOrderCount();
        Task<decimal> GetMonthlyRevenue();
    }
}
