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

        public async Task<IActionResult> OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if(role == "Customer")
            {
                AllOrders = await _orderService.GetAllObjectInfoAsync()
                    .Include(o => o.OrderLines).ThenInclude(ol => ol.Product)
                    .Where(o => o.UserId == HttpContext.Session.GetInt32("UserId")).ToListAsync();

                GrandTotal = 0;
                TotalItems = 0;

                foreach (var order in AllOrders)
                {
                    GrandTotal += order.GetTotalPrice();
                    TotalItems += order.OrderLines.Sum(ol => ol.Quantity);
                }
            }
            else if(role == "Admin")
            {
                AllOrders = await _orderService.GetAllObjectInfoAsync()
                .Include(o => o.OrderLines).ThenInclude(ol => ol.Product).ToListAsync();

            }
            else if(role == "Worker")
            {
                AllOrders = await _orderService.GetAllObjectInfoAsync()
                    .Include(o => o.OrderLines)
                    .Include(o => o.User)
                    .Where(o => o.WorkerId == null && (o.Status == OrderStatus.Processing || o.Status == OrderStatus.Received))
                    .ToListAsync();
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

            var order = await _orderService.GetAllObjectInfoAsync()
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if(order != null && order.WorkerId == null)
            {
                order.WorkerId = workerId;
                order.Status = OrderStatus.OutForDelivery;
                await _orderService.UpdateObjectAsync(order);
            }

            return RedirectToPage("placeholder");

        }

    }
}