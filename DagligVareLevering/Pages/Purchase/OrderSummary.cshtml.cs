using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages.Purchase
{
    public class OrderSummaryModel : PageModel
    {
        private IService<Order> _orderService;

        public OrderSummaryModel(IService<Order> orderService)
        {
            _orderService = orderService;
        }

        public Order? CurrentOrder { get; set; }
        public decimal TotalPrice { get; set; }

        public async Task OnGet()
        {
            int userId = 1; // indtil lennos virker
            // Hent den aktuelle ordre for brugeren, inklusive relaterede data
            CurrentOrder = await _orderService.GetAllObjectInfoAsync()
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.TimeOfOrder)
                .FirstOrDefaultAsync();

            if (CurrentOrder != null)
            {
                TotalPrice = CurrentOrder.GetTotalPrice();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int userId = 1; // indtil lennos virker

            CurrentOrder = await _orderService.GetAllObjectInfoAsync()
                         .Where(o => o.UserId == userId)
                         .OrderByDescending(o => o.TimeOfOrder)
                         .FirstOrDefaultAsync();                


            if (CurrentOrder == null)
            {
                return RedirectToPage("/Purchase/DeliveryTime");
            }

            CurrentOrder.Status = OrderStatus.Processing;

            await _orderService.UpdateObjectAsync(CurrentOrder);
            return RedirectToPage("/Purchase/OrderConfirmation");

        }

    }
}
