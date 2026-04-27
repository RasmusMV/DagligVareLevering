using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace DagligVareLevering.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IService<Order> _dbService;

        public OrderHistoryModel(IService<Order> context)
        {
            _dbService = context;
        }
        public List<Order> AllOrders { get; set; }
        public decimal GrandTotal { get; set; }
        public int TotalItems { get; set; }

        public async Task OnGet()
        {
            AllOrders = await _dbService.GetAllObjectInfoAsync()
                .Include(o => o.OrderLines).ThenInclude(ol => ol.Product).ToListAsync();
            GrandTotal = 0;
            TotalItems = 0;

            foreach (var order in AllOrders)
            {
                GrandTotal += order.GetTotalPrice();
                TotalItems += order.OrderLines.Sum(ol => ol.Quantity);
            }
        }

        public List<Order> GetOrderHistory()
        {
            return AllOrders;
        }
    }
}