using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class OrderConfirmationModel : PageModel
    {
        // Service til at håndtere ordrer
        private readonly IOrderService _orderService;

        public OrderConfirmationModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Models.Order? CurrentOrder { get; set; }
        public decimal TotalPrice { get; set; }

        // OnGet -metoden henter data for den aktuelle ordre, herunder ordrelinjer og tilhørende produkter, og beregner den samlede pris
        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }
            // Hent den seneste ordre for den givne bruger
            CurrentOrder = await _orderService.GetLatestUserOrderAsync(userId.Value);


            if (CurrentOrder == null)
                return RedirectToPage("/OrderFlow/OrderHistory");

            TotalPrice = CurrentOrder.GetTotalPrice();

            return Page();
        }
    }
}