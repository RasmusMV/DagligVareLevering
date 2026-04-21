using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace DagligVareLevering.Pages
{
    public class OrderHistoryModel : PageModel
    {
        public List<Order> AllOrders { get; set; }

        public void OnGet()
        {
            AllOrders = new List<Order>();

            var order = new Order();

            order.OrderLines = new List<OrderLine>();

            order.OrderLines.Add(new OrderLine
            {
                Product = new Product("Mælk", 12m, "Letmælk", null),
                Quantity = 2
            });

            order.OrderLines.Add(new OrderLine
            {
                Product = new Product("Brød", 20m, "Rugbrød", null),
                Quantity = 1
            });

            AllOrders.Add(order);
        }

        public List<Order> GetOrderHistory()
        {
            return AllOrders;
        }
    }
}