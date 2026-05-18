using DagligVareLevering.Handlers;
using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace DagligVareLevering.Pages.OrderFlow
{
    public class OrderHistoryModel : PageModel
    {
        // Services bruges til at hente ordrer og ordrelinjer fra databasen, samt håndtere ordre events
        private readonly IOrderService _orderService;
        private readonly IRepository<OrderLine> _orderLineService;
        private readonly OrderEventsHandler _orderEventsHandler;
        public OrderHistoryModel(IOrderService orderService, IRepository<OrderLine> orderLineService, OrderEventsHandler orderEventsHandler)
        {
            _orderService = orderService;
            _orderLineService = orderLineService;
            _orderEventsHandler = orderEventsHandler;
        }

        // Indenolder de ordrer, der skal vises på siden
        public List<Models.Order> AllOrders { get; set; }
        // Samlet pris for kundens relevante ordrer
        public decimal GrandTotal { get; set; }
        // Samlet antal varer på tværs af kundens relevante ordrer
        public int TotalItems { get; set; }

        public async Task<IActionResult> OnGet()
        {
            // Rollen bestemmer hvilke ordrer brugeren må se
            var role = HttpContext.Session.GetString("UserRole");
            if(role == "Customer")
            {
                // Kunden må kun se sine egne ordrer
                AllOrders = (await _orderService.GetUserOrdersWithOrderLinesAndProducts(HttpContext.Session.GetInt32("UserId").Value)).ToList();

                GrandTotal = 0;
                TotalItems = 0;

                // Beregner totaler for de ordrer, der stadig er relevante i kundens historik
                foreach (var order in AllOrders.Where(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Received 
                || o.Status == OrderStatus.Processing || o.Status == OrderStatus.OutForDelivery || o.Status == OrderStatus.Delayed))
                {
                    GrandTotal += order.GetTotalPrice();
                    TotalItems += order.OrderLines.Sum(ol => ol.Quantity);
                }
            }
            else if(role == "Admin")
            {
                // Admin må se alle ordrer, fordi admin skal kunne administrere og rette fejl
                AllOrders = (await _orderService.GetAllOrdersWithOrderLinesAndProducts()).ToList();

            }
            else if(role == "Worker")
            {
                // Worker ser kun ordrer, der endnu ikke er taget af en leveringsmedarbejder
                AllOrders = (await _orderService.GetAllOrdersWithNoWorker()).ToList();
            }
            else
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostTakeOrderAsync(int orderId)
        {
            // Henter den indloggede workers bruger-id fra sessionen
            int? workerId = HttpContext.Session.GetInt32("UserId");
            if(workerId == null)
            {
                return RedirectToPage("/Login");
            }

            // Knytter ordren til den worker, der tager ordren
            await _orderService.TakeOrderAsync(orderId, workerId.Value);

            // Sender workeren videre til leveringsruten for den valgte ordre
            return RedirectToPage("/OrderFlow/DeliveryRoute");

        }

        public async Task<IActionResult> OnPostDeleteOrderAsync(int orderId)
        {
            var role = HttpContext.Session.GetString("UserRole");

            // Sikrer at kun admin kan slette ordrer
            if (role != "Admin")
            {
                return RedirectToPage("/Login");
            }

            // Finder alle ordrelinjer, der hører til ordren
            List<OrderLine> orderLines = (await _orderLineService.GetObjectsAsync())
                .Where(ol => ol.OrderId == orderId)
                .ToList();

            // Sletter ordrelinjerne først, så selve ordren kan slettes bagefter
            foreach (OrderLine line in orderLines)
            {
                await _orderLineService.DeleteObjectAsync(line);
            }

            // Sletter selve ordren helt fra databasen
            Order deletedOrder = await _orderService.GetObjectByIdAsync(orderId);
            await _orderService.DeleteObjectAsync(deletedOrder);

            TempData["StatusMessage"] = $"Ordre #{orderId} er blevet slettet.";

            return RedirectToPage();
        }

    }
}