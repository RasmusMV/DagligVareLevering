using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class WorkerModel : PageModel
{
    // Service til at hente og opdatere ordredata i databasen
    private IService<Order> _orderService;

    public WorkerModel(IService<Order> orderService)
    {
        _orderService = orderService;
    }

    // Indeholder de aktive ordrer, som leveringsmedarbejderen skal håndtere
    public List<Order> ActiveOrders { get; set; } = new List<Order>();

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Worker")
        {
            return RedirectToPage("/Login");
        }

        // Henter aktive ordrer med kunde, ordrelinjer og produkter
        ActiveOrders = await _orderService.GetAllObjectInfoAsync()
            .Include(o => o.User)
            .Include(o => o.OrderLines)
             .ThenInclude(ol => ol.Product)
             .ThenInclude(p => p.Store)
     .Where(o => o.Status == OrderStatus.Received
              || o.Status == OrderStatus.Processing
              || o.Status == OrderStatus.OutForDelivery)
     .OrderBy(o => o.ExpectedDeliveryTime)
     .ToListAsync();


        return Page();
    }

    public async Task<IActionResult> OnPostMarkDeliveredAsync(int orderId)
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Worker")
        {
            return RedirectToPage("/Login");
        }

        Order? order = await _orderService.GetObjectByIdAsync(orderId);

        if (order != null)
        {
            // Opdaterer status, så ordren ikke længere vises som aktiv
            order.Status = OrderStatus.Delivered;
            await _orderService.UpdateObjectAsync(order);
        }

        return RedirectToPage();
    }
}