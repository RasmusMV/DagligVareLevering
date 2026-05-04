using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
namespace DagligVareLevering.Pages.Purchase
{
    public class OrderHistoryModel : PageModel
    {
        private IService<Models.Order> _orderService;
        private IService<OrderLine> _orderLineService;
        public OrderHistoryModel(IService<Models.Order> orderService, IService<OrderLine> orderLineService)
        {
            _orderService = orderService;
            _orderLineService = orderLineService;
        }
        public List<Models.Order> AllOrders { get; set; }
        public decimal GrandTotal { get; set; }
        public int TotalItems { get; set; }

        public async Task OnGet()
        {
            AllOrders = await _orderService.GetAllObjectInfoAsync()
                .Include(o => o.OrderLines).ThenInclude(ol => ol.Product).ToListAsync();
            GrandTotal = 0;
            TotalItems = 0;

            foreach (var order in AllOrders)
            {
                GrandTotal += order.GetTotalPrice();
                TotalItems += order.OrderLines.Sum(ol => ol.Quantity);
            }
        }

        public List<Models.Order> GetOrderHistory()
        {
            return AllOrders;
        }
    }
}