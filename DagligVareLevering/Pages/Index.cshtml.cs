using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DagligVareLevering.Pages
{
    public class IndexModel : PageModel
    {
        // Service bruges til at hente produkter fra databasen
        private IService<DagligVareLevering.Models.Product> _productService;

        // Service bruges til at lægge produkter i brugerens kurv
        private IService<BasketItem> _basketService;

        // Service bruges til at udløse et event, når en vare lægges i kurven
        private CartEventService _cartEventService;


        public IndexModel(
            IService<DagligVareLevering.Models.Product> productService,
            IService<BasketItem> basketService,
            CartEventService cartEventService)
        {
            _productService = productService;
            _basketService = basketService;
            _cartEventService=cartEventService;
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
            // Henter alle produkter med butik, så forsiden kan vise hvilken butik varen tilhører
            List<DagligVareLevering.Models.Product> allProducts =
                await _productService.GetAllObjectInfoAsync()
                    .Include(p => p.Store)
                    .ToListAsync();

            // Viser de første seks produkter som populære varer på forsiden
            PopularProducts = allProducts
                .Take(6)
                .ToList();

            // Søger kun efter produkter, hvis kunden har skrevet noget i søgefeltet
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResults = allProducts
                    .Where(p => p.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                             || p.Information.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                             || p.Store.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }


        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Hvis brugeren ikke er logget ind, sendes brugeren til login
            if (userId == null)
            {
                return RedirectToPage("/Login");

            }
            // Lytter på eventet og viser en besked, når en vare lægges i kurven
            _cartEventService.CartItemAdded += item =>
            {
                TempData["StatusMessage"] = "Varen er lagt i kurven.";
            };


            // Tjekker om produktet allerede findes i brugerens kurv
            BasketItem? existingItem = (await _basketService.GetObjectsAsync())
                .FirstOrDefault(b => b.UserId == userId.Value && b.ProductId == productId);

            if (existingItem != null)
            {
                // Hvis varen allerede er i kurven, øges antallet
                existingItem.Quantity++;

                // Sikrer at antal ikke bliver højere end den maksimale grænse
                if (existingItem.Quantity > 100)
                {
                    existingItem.Quantity = 100;
                }

                await _basketService.UpdateObjectAsync(existingItem);

                // Udløser eventet, når en eksisterende vare får øget antal i kurven
                _cartEventService.OnCartItemAdded(existingItem);

            }
            else
            {
                // Hvis varen ikke findes i kurven, oprettes en ny BasketItem
                BasketItem basketItem = new BasketItem
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = 1
                };

                await _basketService.AddObjectAsync(basketItem);
                // Udløser eventet, når en ny vare lægges i kurven
                _cartEventService.OnCartItemAdded(basketItem);

            }

            // Sender brugeren tilbage til forsiden og bevarer søgningen
            return RedirectToPage(new { searchText = SearchText });
        }
    }

}
