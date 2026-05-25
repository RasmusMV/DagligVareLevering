using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages
{
    public class IndexModel : PageModel
    {
        // Service bruges til at hente produkter fra databasen
        private readonly IProductService _productService;
        private readonly IBasketItemService _basketItemService;

        public IndexModel(IProductService productService, IBasketItemService basketItemService)
        {
            _productService = productService;
            _basketItemService = basketItemService;
        }

        // Gemmer søgeteksten fra søgefeltet
        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; } = string.Empty;

        // Indeholder de produkter, der matcher kundens søgning
        public List<DagligVareLevering.Models.Product> SearchResults { get; set; }
            = new List<DagligVareLevering.Models.Product>();

        // Indeholder et udvalg af produkter, der vises som populære varer på forsiden
        public List<DagligVareLevering.Models.Product> PopularProducts { get; set; }
            = new List<DagligVareLevering.Models.Product>();

        public async Task OnGet()
        {
            // Viser de første seks produkter som populære varer på forsiden
            PopularProducts = await _productService.GetPopularProductsWithStoreAsync(6);

            // Søger kun efter produkter, hvis kunden har skrevet noget i søgefeltet
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResults = await _productService.SearchProductsAsync(SearchText);
            }
        }


        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Hvis brugeren ikke er logget ind, sendes brugeren til login
            if (userId == null)
            {
                return RedirectToPage("/UserRelated/Login");

            }


            await _basketItemService.AddOrIncrementAsync(userId.Value, productId);

            TempData["StatusMessage"] = "Varen er lagt i kurven.";

            // Sender brugeren tilbage til forsiden og bevarer søgningen
            return RedirectToPage(new { SearchText = SearchText });

        }


    }
  }


