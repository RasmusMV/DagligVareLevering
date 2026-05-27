using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class TrackOrderModel : PageModel
    {// Service bruges til at hente ordredata fra databasen
        private readonly IOrderService _orderService;

        public TrackOrderModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Indeholder den nyeste ordre, som kunden kan følge
        public Order? CurrentOrder { get; set; }

        // Henter den nyeste ordre for brugeren, når siden vises
        public async Task<IActionResult> OnGetAsync(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }

            CurrentOrder = await _orderService.GetObjectByIdAsync(id);
            return Page();
        }

        // Tjekker om et trin i ordrestatussen er nået
        public bool IsStepCompleted(OrderStatus step)
        {
            if (CurrentOrder == null)
            {
                return false;
            }

            // Statusserne er placeret i den rækkefølge, ordren gennemgår dem
            List<OrderStatus> statusOrder = new List<OrderStatus>
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
            // Hvis der ikke er en ordre, kan der ikke annulleres noget
            if (CurrentOrder == null)
            {
                return false;
            }

            // Kunden må kun annullere ordren, hvis den ikke er for langt i leveringsprocessen
            return CurrentOrder.Status == OrderStatus.Received
                || CurrentOrder.Status == OrderStatus.Processing;
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Hvis brugeren ikke er logget ind, sendes brugeren til login
            if (userId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }

            // Henter brugerens nyeste ordre, som forsøges annulleret
            CurrentOrder = await _orderService.GetLatestUserOrderAsync(userId.Value);

            if (CurrentOrder == null)
            {
                return RedirectToPage();
            }

            // Stopper annullering, hvis ordren allerede er på vej eller leveret
            if (!CanCancelOrder())
            {
                TempData["StatusMessage"] = "Ordren kan ikke annulleres, fordi den allerede er på vej eller leveret.";
                return RedirectToPage();
            }

            // Annullerer ordren gennem order service
            await _orderService.CancelOrderAsync(CurrentOrder.OrderId);

            // Viser besked til kunden efter annullering
            TempData["StatusMessage"] = "Din ordre er blevet annulleret.";

            return RedirectToPage();
        }

    }

}

