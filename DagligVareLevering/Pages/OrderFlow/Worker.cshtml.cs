using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class WorkerModel : PageModel
{
    // Service til at hente og opdatere ordrer i databasen
    private readonly IOrderService _orderService;
    // Service til at sende notifikationer
    private IService<Notification> _notificationService;

    public WorkerModel(IOrderService orderService, IService<Notification> notificationService)
    {
        _orderService = orderService;
        _notificationService = notificationService;
    }

    // Indeholder de aktive ordrer, som leveringsmedarbejderen skal håndtere
    public List<Order> ActiveOrders { get; set; } = new List<Order>();

    public async Task<IActionResult> OnGetAsync()
    {
        // Henter brugerens rolle og id fra sessionen
        var role = HttpContext.Session.GetString("UserRole");
        int? workerId = HttpContext.Session.GetInt32("UserId");

        // Sikrer at kun brugere med rollen Worker kan se siden
        if (role != "Worker")
        {
            return RedirectToPage("/UserRelated/Login");
        }

        // Henter kun de ordrer, der er tildelt den indloggede worker
        ActiveOrders = (await _orderService.GetOrdersByWorkerAsync(workerId.Value)).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostMarkDeliveredAsync(int orderId)
    {
        // Henter brugerens rolle og id fra sessionen
        var role = HttpContext.Session.GetString("UserRole");
        int? workerId = HttpContext.Session.GetInt32("UserId");

        // Sikrer at kun workers kan markere en ordre som leveret
        if (role != "Worker")
        {
            return RedirectToPage("/UserRelated/Login");
        }

        // Marker ordren som leveret for den indloggede worker
        await _orderService.MarkOrderAsDeliveredAsync(orderId, workerId.Value);

        return RedirectToPage();
    }
    // Send forsinkelsesnotifikation
    public async Task<IActionResult> OnPostSendDelayAsync(int orderId)
    { 
        // Henter brugerens rolle fra sessionen

        var role = HttpContext.Session.GetString("UserRole");

        // Sikrer at kun workers kan sende forsinkelsesbeskeder
        if (role != "Worker")
        {
            return RedirectToPage("/UserRelated/Login");
        }

        // Henter den ordre, der skal sendes forsinkelsesbesked om
        Order? order = await _orderService.GetObjectByIdAsync(orderId);

        if (order != null)
        {
            // Opretter en notifikation til kunden, der ejer ordren
            Notification notification = new Notification()
            {
                UserId = order.UserId,
                Message = $"Your delivery for order #{order.OrderId} is delayed."
            };

            // Gemmer notifikationen i databasen
            await _notificationService.AddObjectAsync(notification);
        }

        return RedirectToPage();
    }
}