using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Product
{
    public class GetAllProductsModel : PageModel
    {
        private IService<Models.Product> _dbService;
        private IService<BasketItem> _basketService;
        private IService<Models.Store> _storeService;
        public GetAllProductsModel(IService<Models.Product> context, IService<BasketItem> basketService, IService<Models.Store> storeService)
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
            int userId = 1;
            BasketItem newBasketItem = new BasketItem();
            newBasketItem.ProductId = productId;
            newBasketItem.UserId = userId;
            newBasketItem.Quantity = 1;
            await _basketService.AddObjectAsync(newBasketItem);
            return RedirectToPage();
        }
    }
}
