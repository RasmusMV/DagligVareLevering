using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class DeliveryRouteModel : PageModel
    {
        private IOrderService _orderService;
        private IUserService _userService;
        private IConfiguration _configuration;

        public DeliveryRouteModel(IOrderService orderService, IUserService userService, IConfiguration configuration)
        {
            _orderService = orderService;
            _userService = userService;
            _configuration = configuration;
        }

        public Order? CurrentOrder { get; set; }
        public string GoogleMapsApiKey { get; set; }
        public List<string> StoreAdresses { get; set; } = new List<string>();
        public string DeliveryAdress { get; set; }
        public string WorkerAdress { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            int? workerId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            if(workerId == null || role != "Worker")
            {
                return RedirectToPage("/Login");
            }

            CurrentOrder = await _orderService.GetWorkerActiveOrderAsync(workerId.Value, id);

            if (CurrentOrder == null)
            {
                return RedirectToPage("/OrderFlow/OrderHistory");
            }

            GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;

            StoreAdresses = CurrentOrder.OrderLines
                .Select(ol => ol.Product.Store.Adress + ", Danmark")
                .Distinct()
                .ToList();

            DeliveryAdress = CurrentOrder.Adress + ", Danmark";

            var worker = await _userService.GetObjectByIdAsync(workerId.Value);
            WorkerAdress = worker.Adress + ", Danmark";

            return Page();
        }

        public async Task<IActionResult> OnPostMarkDeliveredAsync(int orderId)
        {
            int? workerId = HttpContext.Session.GetInt32("UserId");
            if(workerId == null)
            {
                return RedirectToPage("/Login");
            }

            await _orderService.MarkOrderAsDeliveredAsync(orderId, workerId.Value);

            return RedirectToPage("/OrderFlow/OrderHistory");
        }

    }
}
