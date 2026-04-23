using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages
{
    public class SearchProductModel : PageModel
    {
   
            public List<Product> AllProducts { get; set; }
            public List<Product> FilteredProducts { get; set; }

            [BindProperty]
            public string SearchText { get; set; }

            public void OnGet()
            {
                AllProducts = new List<Product>
            {
                new Product("Mælk", 12m, "Letmælk", null),
                new Product("Brød", 20m, "Rugbrød", null),
                new Product("Smør", 18m, "Lurpak", null),
                new Product("Ost", 25m, "Skiveost", null)
            };

                FilteredProducts = AllProducts;
            }

            public void OnPost()
            {
                AllProducts = new List<Product>
            {
                new Product("Mælk", 12m, "Letmælk", null),
                new Product("Brød", 20m, "Rugbrød", null),
                new Product("Smør", 18m, "Lurpak", null),
                new Product("Ost", 25m, "Skiveost", null)
            };

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    FilteredProducts = AllProducts;
                }
                else
                {
                    FilteredProducts = AllProducts
                        .Where(p => p.Name.ToLower().Contains(SearchText.ToLower()))
                        .ToList();
                }
            }
    }
}
