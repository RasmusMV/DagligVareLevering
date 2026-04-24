using DagligVareLevering.EFDbContext;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DagligVareLevering.Models
{
    public class CartModel : PageModel
    {
        //Service til at håndtere databaseoperationer for produkter og indkøbskurv
        private IService<Product> _productService;
        private IService<BasketItem> _dbService;
        public decimal DeliveryPrice { get; set; }
        public decimal ItemsTotalPrice { get; set; }
        public decimal TotalWithDelivery { get; set; }

        public CartModel(IService<BasketItem> dbService, IService<Product> productService)
        {
            _dbService = dbService;
            _productService=productService;
        }

        // Liste over varer i indkøbskurven, som skal vises på siden
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        // OnGet -metoden henter data for indkøbskurven, herunder hvilke varer der er i kurven, og beregner priserne
        public async Task OnGet()
        {
            int userId = 1; // indtil lennos virker 

            // Hent varer i kurven for den aktuelle bruger og beregn priser
            await LoadCartData(userId);
        }

        // OnPostRemoveAsync -metoden håndterer fjernelse af en vare fra indkøbskurven
        public async Task<IActionResult> OnPostRemoveAsync(int productId)
        {
            // Find det indkøbselement, der skal fjernes, baseret på produktId
            BasketItem? itemToRemove = await _dbService.GetObjectByIdAsync(productId);

            // Hvis elementet findes, slet det fra databasen
            if (itemToRemove != null)
            {
                await _dbService.DeleteObjectAsync(itemToRemove);
            }

            return RedirectToPage();
        }

        // OnPostIncreaseAsync -metoden håndterer forøgelse af mængden af en vare i indkøbskurven
        public async Task<IActionResult> OnPostIncreaseAsync(int productId, int userId)
        {
            // Find det indkøbselement, der skal forøges, baseret på produktId og userId
            BasketItem? itemToIncrease = (await _dbService.GetObjectsAsync())
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            // Hvis elementet findes, forøg mængden og opdater det i databasen
            if (itemToIncrease != null)
            {
                itemToIncrease.Quantity++;
                await _dbService.UpdateObjectAsync(itemToIncrease);
            }

            return RedirectToPage();
        }

        // OnPostDecreaseAsync -metoden håndterer formindskelse af mængden af en vare i indkøbskurven
        public async Task<IActionResult> OnPostDecrease(int productId, int userId)
        {
            // Find det indkøbselement, der skal formindskes, baseret på produktId og userId
            BasketItem? itemToDecrease = (await _dbService.GetObjectsAsync())
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            // Hvis elementet findes, formindsk mængden og opdater det i databasen. Hvis mængden når 0, slet elementet
            if (itemToDecrease != null)
            {
                itemToDecrease.Quantity--;

                if (itemToDecrease.Quantity <= 0)
                {
                    await _dbService.DeleteObjectAsync(itemToDecrease);
                }
               
               await _dbService.UpdateObjectAsync(itemToDecrease);
            }

            return RedirectToPage();
        }

        // LoadCartData -metoden henter indkøbskurvens data for en given bruger, herunder hvilke varer der er i kurven, og beregner priserne
        private async Task LoadCartData(int userId)
        {
            BasketItems = (await _dbService.GetObjectsAsync())
                .Where(b => b.UserId == userId)
                .ToList();

            foreach (var item in BasketItems)
            {
                item.Product = await _productService.GetObjectByIdAsync(item.ProductId);
            }

            DeliveryPrice = 29m;

            ItemsTotalPrice = BasketItems
                .Where(item => item.Product != null)
                .Sum(item => item.Product.Price * item.Quantity);

            TotalWithDelivery = ItemsTotalPrice + DeliveryPrice;
        }
    }
}

