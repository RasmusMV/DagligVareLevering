using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DagligVareLevering.Service;
using Microsoft.IdentityModel.Tokens;

namespace DagligVareLevering.Pages.Product
{
    public class GroceriesModel : PageModel
    {

        private IService<Models.Product> _dbService;
        private IService<BasketItem> _basketService;
        private IService<Models.Store> _storeService;
        public GroceriesModel(IService<Models.Product> context, IService<BasketItem> basketService, IService<Models.Store> storeService)
        {
            _dbService = context;
            _basketService = basketService;
            _storeService = storeService;
        }
        public Models.Product? SelectedProduct { get; set; }
        public Models.Product? ProductStore { get; set; }
        public List<Models.Store> Stores { get; private set; }

        public IList<IGrouping<string, Models.Product>> GroupedProducts { get; set; }

      
        public async Task OnGetAsync(int? id, string? storeName, decimal? maxPrice, int? storeId)
        {
            var products = await _dbService.GetObjectsAsync();
            
            // filtrer før gruppering
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            }

            if (storeId.HasValue)
            {
                products = products.Where(p => p.StoreId == storeId.Value).ToList();
            }
            // sorterer efter pris, laveste først.
            products = products.OrderBy(p => p.Price).ToList();

            // Gruppér EFTER filtrering
            GroupedProducts = products.GroupBy(p => p.Name).ToList();

            Stores = (await _storeService.GetObjectsAsync()).ToList();

            if (id != null)
            {
                SelectedProduct = products.FirstOrDefault(p => p.ProductId == id);
            }

            if (!string.IsNullOrEmpty(storeName))
            {
                ProductStore = products
                    .FirstOrDefault(p => p.Store != null && p.Store.Name == storeName);
            }

        }
        public async Task<IActionResult> OnPostAddToCartAsync(int productId) 
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/Login");
            }
            //Finder første instans hvor userId og productId er i en entity
            var existingBasketItem = await _basketService.GetAllObjectInfoAsync()
                .Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefaultAsync();
            //Hvis der er en entity der har både userId og productId allerede, så øges Quantity propertyen med 1
            if (existingBasketItem != null)
            {
                existingBasketItem.Quantity += 1;
                await _basketService.UpdateObjectAsync(existingBasketItem);
            }
            //Ellers bliver en ny entity lavet med productId og userId
            else
            {
                BasketItem newBasketItem = new BasketItem();
                newBasketItem.ProductId = productId;
                newBasketItem.UserId = userId!.Value;
                newBasketItem.Quantity = 1;
                await _basketService.AddObjectAsync(newBasketItem);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Models.Product product = await _dbService.GetObjectByIdAsync(id);
            await _dbService.DeleteObjectAsync(product);
            return RedirectToPage("/Groceries");
        }
        /*
        // OnPostIncreaseAsync -metoden håndterer forøgelse af mængden af en vare i indkøbskurven
        public async Task<IActionResult> OnPostIncreaseAsync(int productId, int userId)
        {
            BasketItem? itemToIncrease = (await _basketService.GetObjectsAsync())
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            if (itemToIncrease != null)
            {
                itemToIncrease.Quantity++;
                await _basketService.UpdateObjectAsync(itemToIncrease);
            }

            return RedirectToPage();
        }
        */
    }
}
