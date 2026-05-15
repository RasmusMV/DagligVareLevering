using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class WorkerModel : PageModel
{
    private readonly IOrderService _orderService;

    public WorkerModel(IOrderService orderService)
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

        ActiveOrders = (await _orderService.GetOrdersByWorkerAsync(workerId!.Value)).ToList();

        return Page();
    }
}