using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DagligVareLevering.Service;

namespace DagligVareLevering.Pages
{
    public class GroceriesModel : PageModel
    {

        private IService<Product> _dbService;
        private IService<BasketItem> _basketService;

        public GroceriesModel(IService<Product> context, IService<BasketItem> basketService)
        {
            _dbService = context;
            _basketService = basketService;
        }
        public Product? SelectedProduct { get; set; }
        public Product? ProductStore { get; set; }


        public IList<IGrouping<string, Product>> GroupedProducts { get; set; }

        public async Task OnGetAsync(int? id, string? storeName)
        {
            var products = await _dbService.GetObjectsAsync();

            GroupedProducts = products.GroupBy(p => p.Name).ToList();

            if (id != null)
            {
                SelectedProduct = products
                    .FirstOrDefault(p => p.ProductId == id);
            }

            if (!string.IsNullOrEmpty(storeName))
            {
                ProductStore = products
                    .FirstOrDefault(p => p.Store != null && p.Store.Name == storeName);
            }
            

        }
        
        public async Task<IActionResult> OnPostAddToCartAsync(int productId) 
        {
            int userId = 1;
            BasketItem newBasketItem = new BasketItem();
            newBasketItem.ProductId = productId;
            newBasketItem.UserId = userId;
            newBasketItem.Quantity = 1;
            await _basketService.AddObjectAsync(newBasketItem);
            return RedirectToPage();
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
