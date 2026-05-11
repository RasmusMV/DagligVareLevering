using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; 

namespace DagligVareLevering.Pages.OrderFlow
{
    public class TrackOrderModel : PageModel
    {// Service bruges til at hente ordredata fra databasen
        private IService<Order> _orderService;

        public TrackOrderModel(IService<Order> orderService)
        {
            _orderService = orderService;
        }

        // Indeholder den nyeste ordre, som kunden kan følge
        public Order? CurrentOrder { get; set; }

        // Henter den nyeste ordre for brugeren, når siden vises
        public async Task<IActionResult> OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            CurrentOrder = await _orderService.GetAllObjectInfoAsync()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.TimeOfOrder)
                .FirstOrDefaultAsync();
            return Page();
        }

        // Tjekker om et trin i ordrestatussen er nået
        public bool IsStepCompleted(string step)
        {
            if (CurrentOrder == null)
            {
                return false;
            }

            // Statusserne er placeret i den rækkefølge, ordren gennemgår dem
            List<string> statusOrder = new List<string>
            {
                OrderStatus.Received,
                OrderStatus.Processing,
                OrderStatus.OutForDelivery,
                OrderStatus.Delivered
            };

            int currentIndex = statusOrder.IndexOf(CurrentOrder.Status);
            int stepIndex = statusOrder.IndexOf(step);

            // Et trin er gennemført, hvis det ligger før eller på den aktuelle status
            return stepIndex <= currentIndex && stepIndex != -1;
        }

        // Tjekker om ordren stadig kan annulleres
        public bool CanCancelOrder()
        {
            if (CurrentOrder == null)
            {
                return false;
            }

            return CurrentOrder.Status == OrderStatus.Received
                || CurrentOrder.Status == OrderStatus.Processing;
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            CurrentOrder = await _orderService.GetAllObjectInfoAsync()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.TimeOfOrder)
                .FirstOrDefaultAsync();

            if (CurrentOrder == null)
            {
                return RedirectToPage();
            }

            if (!CanCancelOrder())
            {
                TempData["StatusMessage"] = "Ordren kan ikke annulleres, fordi den allerede er på vej eller leveret.";
                return RedirectToPage();
            }

            CurrentOrder.Status = OrderStatus.Cancelled;

            await _orderService.UpdateObjectAsync(CurrentOrder);

            TempData["StatusMessage"] = "Din ordre er blevet annulleret.";

            return RedirectToPage();
        }

    }
}

