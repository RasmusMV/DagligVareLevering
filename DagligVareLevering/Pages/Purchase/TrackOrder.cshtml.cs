using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; 

namespace DagligVareLevering.Pages.Purchase
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
        public async Task OnGet()
        {
            int userId = 1; // Midlertidig testbruger indtil login virker

            CurrentOrder = await _orderService.GetAllObjectInfoAsync()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.TimeOfOrder)
                .FirstOrDefaultAsync();
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
    }
}

