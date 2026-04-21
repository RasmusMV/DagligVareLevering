using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

        public IList<IGrouping<string, Product>> GroupedProducts { get; set; }

        public void OnGet(int? id)
        {
            var products = _context.Products.OrderBy(p => p.Name).ThenBy(p => p.Price).ToList();
            GroupedProducts = products.GroupBy(p => p.Name).ToList();

            if (id != null)
            {
                SelectedProduct = products
                    .FirstOrDefault(p => p.ProductId == id);
            }
        }
    }
}
