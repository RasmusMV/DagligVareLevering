using DagligVareLevering.EFDbContext;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DagligVareLevering.Pages
{
    public class IndexModel : PageModel
    {
        // Service bruges til at hente produkter fra databasen
        private IRepository<DagligVareLevering.Models.Product> _productService;

        public IndexModel(IRepository<DagligVareLevering.Models.Product> productService)
        {
            _productService = productService;
        }

        // Gemmer søgeteksten fra søgefeltet
        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; } = string.Empty;

        // Indeholder de produkter, der matcher kundens søgning
        public List<DagligVareLevering.Models.Product> SearchResults { get; set; }
            = new List<DagligVareLevering.Models.Product>();

        // Indeholder et udvalg af produkter, der vises på forsiden
        public List<DagligVareLevering.Models.Product> PopularProducts { get; set; }
            = new List<DagligVareLevering.Models.Product>();

        public async Task OnGet()
        {
            List<DagligVareLevering.Models.Product> allProducts =
                (await _productService.GetObjectsAsync()).ToList();

            // Viser de første seks produkter som populære varer på forsiden
            PopularProducts = allProducts
                .Take(6)
                .ToList();

            // Søger kun efter produkter, hvis kunden har skrevet noget i søgefeltet
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResults = allProducts
                    .Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                             || p.Information.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}


