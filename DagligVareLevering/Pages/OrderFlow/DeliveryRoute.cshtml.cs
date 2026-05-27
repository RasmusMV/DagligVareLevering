using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class DeliveryRouteModel : PageModel
    {
        // Service bruges til at hente ordre- og brugerdata fra databasen
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Henter den indloggede workers id og rolle fra sessionen
            int? workerId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            // Kun workers må se leveringsruten
            if (workerId == null || role != "Worker")
            {
                return RedirectToPage("/UserRelated/Login");
            }

            // Henter den aktive ordre, som er tildelt den indloggede worker
            CurrentOrder = await _orderService.GetWorkerActiveOrderAsync(workerId.Value, id);

            if (CurrentOrder == null)
            {
                return RedirectToPage("/OrderFlow/OrderHistory");
            }

            // Henter Google Maps API-nøglen
            GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"] ?? string.Empty;

            // Finder de unikke butiksadresser, som workeren skal hente varer fra
            StoreAdresses = CurrentOrder.OrderLines
                .Select(ol => ol.Product.Store.Adress + ", Danmark")
                .Distinct()
                .ToList();

            // Sætter kundens leveringsadresse som slutpunkt
            DeliveryAdress = CurrentOrder.Adress + ", Danmark";

            // Henter workerens adresse som startpunkt for ruten
            var worker = await _userService.GetObjectByIdAsync(workerId.Value);
            WorkerAdress = worker.Adress + ", Danmark";

            return Page();
        }

        public async Task<IActionResult> OnPostMarkDeliveredAsync(int orderId)
        {
            // Henter den indloggede workers id fra sessionen
            int? workerId = HttpContext.Session.GetInt32("UserId");
            if(workerId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }

            // Marker ordren som leveret
            await _orderService.MarkOrderAsDeliveredAsync(orderId, workerId.Value);

            return RedirectToPage("/OrderFlow/OrderHistory");
        }

    }
}
