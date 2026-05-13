using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class WorkerModel : PageModel
{
    private IService<Order> _orderService;

    public WorkerModel(IService<Order> orderService)
    {
        _orderService = orderService;
    }

    public List<Order> ActiveOrders { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("UserRole");
        int? workerId = HttpContext.Session.GetInt32("UserId");
        if (role != "Worker")
        {
            return RedirectToPage("/Login");
        }

        ActiveOrders = await _orderService.GetAllObjectInfoAsync()
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.Product)
            .Include(o => o.User)
            .Where(o => o.WorkerId == workerId && o.Status == OrderStatus.OutForDelivery)
            .ToListAsync();

        return Page();
    }
}