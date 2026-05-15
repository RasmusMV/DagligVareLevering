using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DagligVareLevering.Pages
{
    public class IndexModel : PageModel
    {
        // Service bruges til at hente produkter fra databasen
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
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

            // Viser de første seks produkter som populære varer på forsiden
            PopularProducts = await _productService.GetPopularProductsAsync(6);

            // Søger kun efter produkter, hvis kunden har skrevet noget i søgefeltet
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResults = await _productService.SearchProductsAsync(SearchText);
            }
        }
    }
}


