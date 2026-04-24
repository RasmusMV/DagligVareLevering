using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DagligVareLevering.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        private AppDbContext _context;
        public OrderConfirmationModel(AppDbContext context)
        {
            _context = context;
        }

        public Order? CurrentOrder { get; set; }
        public decimal TotalPrice { get; set; }

        public void OnGet()
        {
            int userId = 1; // midlertidigt indtil login er lavet
            CurrentOrder = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderLines)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.TimeOfOrder)
                .FirstOrDefault();

            if (CurrentOrder != null)
            {
                TotalPrice = CurrentOrder.OrderLines.Sum(orderLine => orderLine.GetLineTotal());
            }
        }
    }
}
