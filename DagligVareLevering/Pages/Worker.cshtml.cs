using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class WorkerModel : PageModel
{
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
        var role = HttpContext.Session.GetString("UserRole");
        int? workerId = HttpContext.Session.GetInt32("UserId");

        if (role != "Worker")
        {
            return RedirectToPage("/Login");
        }

        ActiveOrders = (await _orderService.GetOrdersByWorkerAsync(workerId.Value)).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostMarkDeliveredAsync(int orderId)
    {
        var role = HttpContext.Session.GetString("UserRole");
        int? workerId = HttpContext.Session.GetInt32("UserId");
        if (role != "Worker")
        {
            return RedirectToPage("/Login");
        }

        await _orderService.MarkOrderAsDeliveredAsync(orderId, workerId.Value);

        return RedirectToPage();
    }
    // Send forsinkelsesnotifikation
    public async Task<IActionResult> OnPostSendDelayAsync(int orderId)
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Worker")
        {
            return RedirectToPage("/Login");
        }

        Order? order = await _orderService.GetObjectByIdAsync(orderId);

        if (order != null)
        {
            Notification notification = new Notification()
            {
                UserId = order.UserId,
                Message = $"Your delivery for order #{order.OrderId} is delayed."
            };

            await _notificationService.AddObjectAsync(notification);
        }

        return RedirectToPage();
    }
}