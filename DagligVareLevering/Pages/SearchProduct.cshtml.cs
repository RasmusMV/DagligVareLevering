using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages
{
    public class SearchProductModel : PageModel
    {
        // Indeholder alle produkter, som kan vises på siden
        public List<Models.Product> AllProducts { get; set; }

        // Indeholder de produkter, der matcher kundens søgning
        public List<Models.Product> FilteredProducts { get; set; }

        // Gemmer den tekst, brugeren skriver i søgefeltet
        [BindProperty]
            public string SearchText { get; set; }

            public void OnGet()
            {
                AllProducts = new List<Models.Product>
            {
                new Models.Product("Mælk", 12m, "Letmælk", null),
                new Models.Product("Brød", 20m, "Rugbrød", null),
                new Models.Product("Smør", 18m, "Lurpak", null),
                new Models.Product("Ost", 25m, "Skiveost", null)
            };

                FilteredProducts = AllProducts;
            }

            public void OnPost()
            {
                AllProducts = new List<Models.Product>
            {
                new Models.Product("Mælk", 12m, "Letmælk", null),
                new Models.Product("Brød", 20m, "Rugbrød", null),
                new Models.Product("Smør", 18m, "Lurpak", null),
                new Models.Product("Ost", 25m, "Skiveost", null)
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
