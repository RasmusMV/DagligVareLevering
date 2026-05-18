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

       
       
       
        //indeholder de produkter, der skal vises på siden
        public List<DagligVareLevering.Models.Product> Products { get; set; }
            = new List<DagligVareLevering.Models.Product>();

        public async Task OnGet()
        {
            Stores = (await _storeService.GetObjectsAsync()).ToList();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                // Viser kun produkter, der matcher søgningen
                Products = await _productService.SearchProductsAsync(SearchText);
            }
            else
            {
                // Viser alle produkter, hvis kunden ikke har søgt
                Products = (await _productService.GetObjectsAsync()).ToList();
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
