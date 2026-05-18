using DagligVareLevering.Handlers;
using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace DagligVareLevering.Pages.OrderFlow
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IRepository<OrderLine> _orderLineService;
        private readonly OrderEventsHandler _orderEventsHandler;
        public OrderHistoryModel(IOrderService orderService, IRepository<OrderLine> orderLineService, OrderEventsHandler orderEventsHandler)
        {
            _orderService = orderService;
            _orderLineService = orderLineService;
            _orderEventsHandler = orderEventsHandler;
        }
        public List<Models.Order> AllOrders { get; set; }
        public decimal GrandTotal { get; set; }
        public int TotalItems { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if(role == "Customer")
            {
                AllOrders = (await _orderService.GetUserOrdersWithOrderLinesAndProducts(HttpContext.Session.GetInt32("UserId").Value)).ToList();

                GrandTotal = 0;
                TotalItems = 0;

                foreach (var order in AllOrders.Where(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Received 
                || o.Status == OrderStatus.Processing || o.Status == OrderStatus.OutForDelivery || o.Status == OrderStatus.Delayed))
                {
                    GrandTotal += order.GetTotalPrice();
                    TotalItems += order.OrderLines.Sum(ol => ol.Quantity);
                }
            }
            else if(role == "Admin")
            {
                AllOrders = (await _orderService.GetAllOrdersWithOrderLinesAndProducts()).ToList();

            }
            else if(role == "Worker")
            {
                AllOrders = (await _orderService.GetAllOrdersWithNoWorker()).ToList();
            }
            else
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostTakeOrderAsync(int orderId)
        {
            int? workerId = HttpContext.Session.GetInt32("UserId");
            if(workerId == null)
            {
                return RedirectToPage("/Login");
            }

            await _orderService.TakeOrderAsync(orderId, workerId.Value);

            return RedirectToPage("/OrderFlow/DeliveryRoute");

        }

    }
}