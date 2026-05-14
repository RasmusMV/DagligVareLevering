using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Service
{
    public class OrderService : GenericService<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository) : base(orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<Order> GetLatestUserOrderAsync(int userId)
        {
            return _orderRepository.GetLatestUserOrderAsync(userId);
        }

        public Task<Order?> GetWorkerActiveOrderAsync(int workerId, int orderId)
        {
            return _orderRepository.GetWorkerActiveOrderAsync(workerId, orderId);
        }

        public Task<IEnumerable<Order>> GetOrdersByWorkerAsync(int workerId)
        {
            return _orderRepository.GetOrdersByWorkerAsync(workerId);
        }
        public Task<IEnumerable<Order>> GetUserOrdersWithOrderLinesAndProducts(int userId)
        {
            return _orderRepository.GetUserOrdersWithOrderLinesAndProducts(userId);
        }

        public Task<IEnumerable<Order>> GetAllOrdersWithOrderLinesAndProducts()
        {
            return _orderRepository.GetAllOrdersWithOrderLinesAndProducts();
        }

        public Task<IEnumerable<Order>> GetAllOrdersWithNoWorker()
        {
            return _orderRepository.GetAllOrdersWithNoWorker();
        }

        public async Task MarkOrderAsDeliveredAsync(int orderId, int workerId)
        {
            var order = await GetObjectByIdAsync(orderId);

            if(order == null || order.WorkerId != workerId)
            {
                throw new UnauthorizedAccessException("Order ikke fundet, eller forkert user");
            }

            order.Status = OrderStatus.Delivered;
            await UpdateObjectAsync(order);
        }

        public async Task TakeOrderAsync(int orderId, int workerId)
        {
            var order = await GetObjectByIdAsync(orderId);

            if (order != null && order.WorkerId == null)
            {
                order.WorkerId = workerId;
                order.Status = OrderStatus.OutForDelivery;
                await UpdateObjectAsync(order);
            }
        }

        public async Task ConfirmOrderAsync(int userId, string? deliveryAdress, string paymentMethod)
        {
            var order = await GetLatestUserOrderAsync(userId);

            // Opdaterer kun leveringsadressen, hvis brugeren har skrevet en ny adresse
            if (!string.IsNullOrWhiteSpace(deliveryAdress))
            {
                order.Adress = deliveryAdress;
            }

            order.PaymentMethod = paymentMethod;
            order.Status = OrderStatus.Received;

            // Gemmer ændringerne i databasen
            await UpdateObjectAsync(order);

        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await GetObjectByIdAsync(orderId);
            order.Status = OrderStatus.Cancelled;
            await UpdateObjectAsync(order);
        }

        public async Task<IEnumerable<Order>> SortById()
        {
            return (await GetObjectsAsync()).OrderBy(x => x.OrderId);
        }

        public async Task<IEnumerable<Order>> SortByIdDescending()
        {
            return (await GetObjectsAsync()).OrderByDescending(x => x.OrderId);
        }

        public async Task<IEnumerable<Order>> SortByTotalPrice()
        {
            return (await GetObjectsAsync()).OrderBy(x => x.GetTotalPrice());
        }

        public async Task<int> GetTotalOrders()
        {
            return (await GetObjectsAsync()).Count();
        }

        public async Task<decimal> GetTotalRevenue()
        {
            return (await GetObjectsAsync()).Sum(x => x.GetTotalPrice() + x.DeliveryPrice);
        }

        public async Task<int> GetMonthlyOrderCount()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            return (await GetObjectsAsync()).Where(x => x.TimeOfOrder >= lastMonth).Count();
        }

        public async Task<decimal> GetMonthlyRevenue()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            return (await GetObjectsAsync())
                .Where(x => x.TimeOfOrder >= lastMonth)
                .Sum(x => x.GetTotalPrice() + x.DeliveryPrice);
        }

        
    }
}
