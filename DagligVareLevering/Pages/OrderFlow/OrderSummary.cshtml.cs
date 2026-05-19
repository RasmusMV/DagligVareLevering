using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class OrderSummaryModel : PageModel
    {
        // Service til at håndtere databaseoperationer for ordrer
        private readonly IOrderService _orderService;
        private readonly IBasketItemService _basketItemService;
        public OrderSummaryModel(IOrderService orderService, IBasketItemService basketItemService)
        {
            _orderService = orderService;
            _basketItemService = basketItemService;
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
                return RedirectToPage("/UserRelated/Login");
            }
            // Hent den aktuelle ordre for brugeren, inklusive relaterede data
            CurrentOrder = await _orderService.GetLatestUserOrderAsync(userId.Value);


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
                return RedirectToPage("/UserRelated/Login");
            }

            var order = await _orderService.GetLatestUserOrderAsync(userId.Value);

            if (order == null)
            {
                return RedirectToPage("/OrderFlow/DeliveryTime");
            }

            await _orderService.ConfirmOrderAsync(userId.Value, DeliveryAddress, PaymentMethod);
            // Fjerner BasketItem fra kurven
            await _basketItemService.ClearBasketAsync(userId.Value);

            return RedirectToPage("/OrderFlow/OrderConfirmation");

        }

    }
}
