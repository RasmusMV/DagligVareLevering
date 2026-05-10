using System.Runtime.CompilerServices;
using DagligVareLevering.Models;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Service
{
    public class OrderService : IOrderService
    {
        private readonly IService<Order> _orderService;

        public OrderService(IService<Order> orderService)
        {
            _orderService = orderService;
        }

        public async Task<IEnumerable<Order>> SortById()
        {
            return (await _orderService.GetObjectsAsync()).OrderBy(x => x.OrderId);
        }

        public async Task<IEnumerable<Order>> SortByIdDescending()
        {
            return (await _orderService.GetObjectsAsync()).OrderByDescending(x => x.OrderId);
        }

        public async Task<IEnumerable<Order>> SortByTotalPrice()
        {
            return (await _orderService.GetObjectsAsync()).OrderBy(x => x.GetTotalPrice());
        }

        public async Task<int> GetTotalOrders()
        {
            return (await _orderService.GetObjectsAsync()).Count();
        }

        public async Task<decimal> GetTotalRevenue()
        {
            return (await _orderService.GetObjectsAsync()).Sum(x => x.GetTotalPrice() + x.DeliveryPrice);
        }

        public async Task<int> GetMonthlyOrderCount()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            return (await _orderService.GetObjectsAsync()).Where(x => x.TimeOfOrder >= lastMonth).Count();
        }

        public async Task<decimal> GetMonthlyRevenue()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            return (await _orderService.GetObjectsAsync()).Where(x => x.TimeOfOrder >= lastMonth).Sum(x => x.GetTotalPrice() + x.DeliveryPrice);
        }
    }
}
