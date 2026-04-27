using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace DagligVareLevering.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private IService<Order> _orderService;
        private IService<OrderLine> _orderLineService;
        public OrderHistoryModel(IService<Order> orderService, IService<OrderLine> orderLineService)
        {
            _orderService = orderService;
            _orderLineService = orderLineService;
        }
        public List<Order> AllOrders { get; set; }
        public decimal GrandTotal { get; set; }
        public int TotalItems { get; set; }

        public async Task OnGet()
        {
            AllOrders = (await _orderService.GetObjectsAsync()).ToList();
            GrandTotal = 0;
            TotalItems = 0;

            foreach (var order in AllOrders)
            {
                GrandTotal += order.GetTotalPrice();
                TotalItems += order.OrderLines.Sum(ol => ol.Quantity);
            }
        }

        public List<Order> GetOrderHistory()
        {
            return AllOrders;
        }
    }
}