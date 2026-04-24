using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        // Services til at håndtere databaseoperationer for ordrer, ordrelinjer og produkter
        private IService<Order> _orderService;
        private IService<Product> _productService;
        private IService<OrderLine> _orderLineService;

        public OrderConfirmationModel(IService<Order> ordreService, IService<OrderLine> orderLineService, IService<Product> productService)
        {
            _orderService = ordreService;
            _orderLineService = orderLineService;
            _productService = productService;
        }

        public Order? CurrentOrder { get; set; }
        public decimal TotalPrice { get; set; }

        // OnGet -metoden henter data for den aktuelle ordre, herunder ordrelinjer og tilhørende produkter, og beregner den samlede pris
        public async Task OnGet()
        {
            int userId = 1;
            // Hent den seneste ordre for den givne bruger
            CurrentOrder = (await _orderService.GetObjectsAsync())
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.TimeOfOrder)
                .FirstOrDefault();

            if (CurrentOrder == null)
                return;

            // Hent ordrelinjerne for den aktuelle ordre
            CurrentOrder.OrderLines = (await _orderLineService.GetObjectsAsync())
                .Where(ol => ol.OrderId == CurrentOrder.OrderId)
                .ToList();

            // For hver ordrelinje, hent det tilhørende produkt
            foreach (var line in CurrentOrder.OrderLines)
            {
                line.Product = await _productService.GetObjectByIdAsync(line.ProductId);
            }

            TotalPrice = CurrentOrder.OrderLines.Sum(line => line.GetLineTotal());
        }
    }
}