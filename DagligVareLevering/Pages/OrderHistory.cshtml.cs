using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace DagligVareLevering.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly AppDbContext _context;

        public OrderHistoryModel(AppDbContext context)
        {
            _context = context;
        }
        public List<Order> AllOrders { get; set; }
        public decimal GrandTotal { get; set; }
        public int TotalItems { get; set; }

        public void OnGet()
        {
            AllOrders = new List<Order>();

            var order1 = new Order();
            order1.OrderLines = new List<OrderLine>();

            order1.OrderLines.Add(new OrderLine
            {
                Product = new Product("Mælk", 12m, "Letmælk", null),
                Quantity = 2
            });

            order1.OrderLines.Add(new OrderLine
            {
                Product = new Product("Brød", 20m, "Rugbrød", null),
                Quantity = 1
            });

            var order2 = new Order();
            order2.OrderLines = new List<OrderLine>();

            order2.OrderLines.Add(new OrderLine
            {
                Product = new Product("Smør", 18m, "Lurpak", null),
                Quantity = 1
            });

            order2.OrderLines.Add(new OrderLine
            {
                Product = new Product("Ost", 25m, "Skiveost", null),
                Quantity = 2
            });

            AllOrders.Add(order1);
            AllOrders.Add(order2);
                      
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