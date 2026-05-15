using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class OrderConfirmationModel : PageModel
    {
        // Services til at håndtere databaseoperationer for ordrer, ordrelinjer og produkter
        private readonly IOrderService _orderService;
        private readonly IRepository<Models.Product> _productService;
        private readonly IRepository<OrderLine> _orderLineService;

        public OrderConfirmationModel(IOrderService orderService, IRepository<OrderLine> orderLineService, IRepository<Models.Product> productService)
        {
            _orderService = orderService;
            _orderLineService = orderLineService;
            _productService = productService;
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
            CurrentOrder = await _orderService.GetLatestUserOrderAsync(userId.Value);


            if (CurrentOrder == null)
                return RedirectToPage("/OrderHistory");

            // Hent ordrelinjerne for den aktuelle ordre
            CurrentOrder.OrderLines = (await _orderLineService.GetObjectsAsync())
                .Where(ol => ol.OrderId == CurrentOrder.OrderId)
                .ToList();

            // For hver ordrelinje, hent det tilhørende produkt
            foreach (var line in CurrentOrder.OrderLines)
            {
                line.Product = await _productService.GetObjectByIdAsync(line.ProductId);
            }

            TotalPrice = CurrentOrder.GetTotalPrice();

            return Page();
        }
    }
}