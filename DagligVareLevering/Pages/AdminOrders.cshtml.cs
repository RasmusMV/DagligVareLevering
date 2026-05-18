using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages
{
    public class AdminOrdersModel : PageModel
    {
        // Service bruges til at hente og slette ordrer
        private readonly IService<Order> _orderService;

        // Service bruges til at hente og slette ordrelinjer
        private readonly IService<OrderLine> _orderLineService;


        // Service bruges til at hente produkter til ordrelinjerne
        private readonly IService<DagligVareLevering.Models.Product> _productService;

        // Service bruges til at hente kunden på ordren
        private readonly IService<User> _userService;

        public AdminOrdersModel(
          IService<Order> orderService,
          IService<OrderLine> orderLineService,
          IService<DagligVareLevering.Models.Product> productService,
          IService<User> userService)
        {
            _orderService = orderService;
            _orderLineService = orderLineService;
            _productService = productService;
            _userService = userService;
        }

        // Indeholder alle ordrer, som admin kan administrere
        public List<Order> Orders { get; set; } = new List<Order>();

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                return RedirectToPage("/Login");
            }

            // Henter alle ordrer
            Orders = (await _orderService.GetObjectsAsync())
                .OrderByDescending(o => o.TimeOfOrder)
                .ToList();

            // Henter kunde, ordrelinjer og produkter manuelt, fordi GenericService ikke bruger Include her
            foreach (Order order in Orders)
            {
                order.User = await _userService.GetObjectByIdAsync(order.UserId);

                order.OrderLines = (await _orderLineService.GetObjectsAsync())
                    .Where(ol => ol.OrderId == order.OrderId)
                    .ToList();

                foreach (OrderLine line in order.OrderLines)
                {
                    line.Product = await _productService.GetObjectByIdAsync(line.ProductId);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteOrderAsync(int orderId)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                return RedirectToPage("/Login");
            }

            // Finder ordren, der skal slettes
            Order? order = await _orderService.GetObjectByIdAsync(orderId);

            if (order == null)
            {
                TempData["StatusMessage"] = "Ordren blev ikke fundet.";
                return RedirectToPage();
            }

            // Finder alle ordrelinjer, der hører til ordren
            List<OrderLine> orderLines = (await _orderLineService.GetObjectsAsync())
                .Where(ol => ol.OrderId == orderId)
                .ToList();

            // Sletter ordrelinjerne først, fordi de afhænger af ordren
            foreach (OrderLine line in orderLines)
            {
                await _orderLineService.DeleteObjectAsync(line);
            }

            // Sletter selve ordren helt fra databasen
            await _orderService.DeleteObjectAsync(order);

            TempData["StatusMessage"] = $"Ordre #{orderId} er blevet slettet.";

            return RedirectToPage();
        }
    }

}
