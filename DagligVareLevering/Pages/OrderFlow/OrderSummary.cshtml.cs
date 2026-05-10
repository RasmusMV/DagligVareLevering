using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages.Purchase
{
    public class OrderSummaryModel : PageModel
    {
        // Service til at håndtere databaseoperationer for ordrer
        private IService<Order> _orderService;

        public OrderSummaryModel(IService<Order> orderService)
        {
            _orderService = orderService;
        }

        // Indeholder den aktuelle ordre, som kunden er ved at gennemføre
        public Order? CurrentOrder { get; set; }
        public decimal TotalPrice { get; set; }
        [BindProperty]
        public string PaymentMethod { get; set; } = string.Empty;
        // Gemmer leveringsadressen fra formularen, hvis brugeren ændrer den
        [BindProperty]
        public string DeliveryAddress { get; set; } = string.Empty;



        // Henter den nyeste ordre for brugeren og viser den som et resume
        public async Task<IActionResult> OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }
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

                // Viser den adresse, der allerede er gemt på orderen
                DeliveryAddress = CurrentOrder.Adress;
            }
            return Page();
        }

        // Gemmer betalingsform, markerer orderen som modtaget og sender brugeren videre til kvittering
        public async Task<IActionResult> OnPostAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            CurrentOrder = await _orderService.GetAllObjectInfoAsync()
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.TimeOfOrder)
            .FirstOrDefaultAsync();



            if (CurrentOrder == null)
            {
                return RedirectToPage("/OrderFlow/DeliveryTime");
            }

            // Opdaterer kun leveringsadressen, hvis brugeren har skrevet en ny adresse
            if (!string.IsNullOrWhiteSpace(DeliveryAddress))
            {
                CurrentOrder.Adress = DeliveryAddress;
            }

            CurrentOrder.PaymentMethod = PaymentMethod;
            CurrentOrder.Status = OrderStatus.Received;

            // Gemmer ændringerne i databasen
            await _orderService.UpdateObjectAsync(CurrentOrder);
            return RedirectToPage("/OrderFlow/OrderConfirmation");

        }

    }
}
