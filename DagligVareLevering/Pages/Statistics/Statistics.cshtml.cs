using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages.Statistics
{
    public class StatisticsModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public StatisticsModel(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public int TotalOrders { get; set; }

        public decimal TotalBoughtFor { get; set; }

        public int TotalUsers { get; set; }

        public int MonthlyOrders { get; set; }

        public decimal MonthlyRevenue { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToPage("/Login");
            }
                
            TotalOrders = await _orderService.GetTotalOrders();
            TotalBoughtFor = await _orderService.GetTotalRevenue();
            TotalUsers = await _userService.GetTotalUsersAsync();
            MonthlyOrders = await _orderService.GetMonthlyOrderCount();
            MonthlyRevenue = await _orderService.GetMonthlyRevenue();
            return Page();
        }


    }
}
