using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace DagligVareLevering.Pages
{
    public class GroceriesModel : PageModel
    {

        private readonly AppDbContext _context;

        public GroceriesModel(AppDbContext context)
        {
            _context = context;
        }
        public Product? SelectedProduct { get; set; }
        public Product? ProductStore { get; set; }

        public IList<IGrouping<string, Product>> GroupedProducts { get; set; }

        public void OnGet(int? id, string? storeName)
        {
            var products = _context.Products
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Price).ThenBy(p => p.Store.Name)
                .ToList();

            GroupedProducts = products.GroupBy(p => p.Name).ToList();

            if (id != null)
            {
                SelectedProduct = products
                    .FirstOrDefault(p => p.ProductId == id);
            }

            if (!string.IsNullOrEmpty(storeName))
            {
                ProductStore = products
                    .FirstOrDefault(p => p.Store != null && p.Store.Name == storeName);
            }

        }

        public IActionResult OnPostIncrease(int productId)
        {
            int userId = 1;

            BasketItem? itemToIncrease = _context.BasketItems
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            if (itemToIncrease != null)
            {
                itemToIncrease.Quantity++;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
