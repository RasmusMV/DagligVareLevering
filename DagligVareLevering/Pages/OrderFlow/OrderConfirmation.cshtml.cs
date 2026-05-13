using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class OrderConfirmationModel : PageModel
    {
        // Services til at håndtere databaseoperationer for ordrer, ordrelinjer og produkter
        private IService<Models.Order> _orderService;
   

        public OrderConfirmationModel(IService<Models.Order> ordreService)
        {
            _orderService = ordreService; 
        }

        public Models.Order? CurrentOrder { get; set; }
        public decimal TotalPrice { get; set; }

        // OnGet -metoden henter data for den aktuelle ordre, herunder ordrelinjer og tilhørende produkter, og beregner den samlede pris
        public async Task<IActionResult> OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                return RedirectToPage("/Login");
            }
            // Hent den seneste ordre for den givne bruger
            CurrentOrder = await _orderService.GetAllObjectInfoAsync()
             .Include(o => o.User)
             .Include(o => o.OrderLines)
             .ThenInclude(ol => ol.Product)
             .Where(o => o.UserId == userId.Value)
             .OrderByDescending(o => o.TimeOfOrder)
             .FirstOrDefaultAsync();


            if (CurrentOrder == null)
                return RedirectToPage("/OrderHistory");

            TotalPrice = CurrentOrder.GetTotalPrice();

            return Page();
        }
    }
}