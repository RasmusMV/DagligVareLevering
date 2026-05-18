using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;

namespace DagligVareLevering.Pages.Product
{
    public class GroceriesModel : PageModel
    {

        private readonly IProductService _productService;
        private readonly IBasketItemService _basketService;
        private readonly IRepository<Models.Store> _storeService;
        public GroceriesModel(IProductService productService, IBasketItemService basketService, IRepository<Models.Store> storeService)
        {
            _productService = productService;
            _basketService = basketService;
            _storeService = storeService;
        }
        // Gemmer søgeteksten fra søgefeltet
        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; } = string.Empty;

        // Indeholder de produkter, der matcher kundens søgning
        public List<DagligVareLevering.Models.Product> SearchResults { get; set; }
            = new List<DagligVareLevering.Models.Product>();

        public Models.Product? SelectedProduct { get; set; }
        public List<Models.Store> Stores { get; private set; }
        public Dictionary<string, List<Models.Product>> GroupedProducts { get; set; }

      
        public async Task OnGetAsync(int? id, string? storeName, decimal? maxPrice, int? storeId)
        {
            GroupedProducts = await _productService.GetGroupedProductsAsync(maxPrice, storeId);
            Stores = (await _storeService.GetObjectsAsync()).ToList();
            // Søger kun efter produkter, hvis kunden har skrevet noget i søgefeltet
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResults = await _productService.SearchProductsAsync(SearchText);
            }

            if (id != null)
            {
                SelectedProduct = GroupedProducts
                    .SelectMany(g => g.Value)
                    .FirstOrDefault(p => p.ProductId == id);
            }

            

        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId) 
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }
            
            await _basketService.AddOrIncrementAsync(userId.Value, productId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Models.Product product = await _productService.GetObjectByIdAsync(id);
            await _productService.DeleteObjectAsync(product);
            return RedirectToPage("/Groceries");
        }

    }
}
