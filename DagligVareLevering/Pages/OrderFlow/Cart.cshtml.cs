using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class CartModel : PageModel
    {
        //Service til at håndtere databaseoperationer for produkter og indkøbskurv
        private IProductService _productService;
        private IBasketItemService _basketItemService;
        private IOrderService _orderService;
        private IRepository<OrderLine> _orderLineService;
        private IUserService _userService;


        public decimal DeliveryPrice { get; set; }
        public decimal ItemsTotalPrice { get; set; }
        public decimal TotalWithDelivery { get; set; }

        public CartModel(
        IBasketItemService basketItemService,
        IProductService productService,
        IOrderService orderService,
        IRepository<OrderLine> orderLineService,
        IUserService userService)
        {
            _basketItemService = basketItemService;
            _productService = productService;
            _orderService = orderService;
            _orderLineService = orderLineService;
            _userService = userService;
        }


        // Liste over varer i indkøbskurven, som skal vises på siden
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        // OnGet -metoden henter data for indkøbskurven, herunder hvilke varer der er i kurven, og beregner priserne
        public async Task<IActionResult> OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Hent varer i kurven for den aktuelle bruger og beregn priser
            await LoadCartData(userId!.Value);
            return Page();
        }

        // OnPostRemoveAsync -metoden håndterer fjernelse af en vare fra indkøbskurven
        public async Task<IActionResult> OnPostRemoveAsync(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Find det indkøbselement, der skal fjernes, baseret på produktId
            BasketItem? itemToRemove = (await _basketItemService.GetObjectsAsync())
                  .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            // Hvis elementet findes, slet det fra databasen
            if (itemToRemove != null)
            {
                await _basketItemService.DeleteObjectAsync(itemToRemove);
            }

            return RedirectToPage();
        }

        // OnPostIncreaseAsync -metoden håndterer forøgelse af mængden af en vare i indkøbskurven
        public async Task<IActionResult> OnPostIncreaseAsync(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Find det indkøbselement, der skal forøges, baseret på produktId og userId
            BasketItem? itemToIncrease = (await _basketItemService.GetObjectsAsync())
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            // Hvis elementet findes, forøg mængden og opdater det i databasen
            if (itemToIncrease != null && itemToIncrease.Quantity < 100)
            {
                itemToIncrease.Quantity++;
                await _basketItemService.UpdateObjectAsync(itemToIncrease);
            }

            return RedirectToPage();
        }

        // OnPostDecreaseAsync -metoden håndterer formindskelse af mængden af en vare i indkøbskurven
        public async Task<IActionResult> OnPostDecreaseAsync(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Find det indkøbselement, der skal formindskes, baseret på produktId og userId
            BasketItem? itemToDecrease = (await _basketItemService.GetObjectsAsync())
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            // Hvis elementet findes, formindsk mængden og opdater det i databasen. Hvis mængden når 0, slet elementet
            if (itemToDecrease != null)
            {
                itemToDecrease.Quantity--;

                if (itemToDecrease.Quantity <= 0)
                {
                    await _basketItemService.DeleteObjectAsync(itemToDecrease);
                }
                else
                {
                    await _basketItemService.UpdateObjectAsync(itemToDecrease);
                }
               
              
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            User user = await _userService.GetObjectByIdAsync(userId!.Value);

            BasketItems = (await _basketItemService.GetObjectsAsync())
                .Where(b => b.UserId == userId)
                .ToList();

            if (BasketItems.Count == 0)
            {
                return RedirectToPage();
            }

            Order order = new Order
            {
                UserId = userId!.Value,
                Adress = user.Adress,
                DeliveryPrice = 29m,
            };

            await _orderService.AddObjectAsync(order);

            foreach (BasketItem item in BasketItems)
            {
                OrderLine orderLine = new OrderLine
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                await _orderLineService.AddObjectAsync(orderLine);
            }

            return RedirectToPage("/OrderFlow/DeliveryTime");
        }


        // LoadCartData -metoden henter indkøbskurvens data for en given bruger, herunder hvilke varer der er i kurven, og beregner priserne
        private async Task LoadCartData(int userId)
        {
            BasketItems = (await _basketItemService.GetObjectsAsync())
                .Where(b => b.UserId == userId)
                .ToList();

            foreach (var item in BasketItems)
            {
                item.Product = await _productService.GetObjectByIdAsync(item.ProductId);
            }

            DeliveryPrice = BasketItems.Any() ? 29m : 0m; // Fast leveringspris, hvis der er varer i kurven

            ItemsTotalPrice = BasketItems
                .Where(item => item.Product != null)
                .Sum(item => item.Product.Price * item.Quantity);

            TotalWithDelivery = ItemsTotalPrice + DeliveryPrice;
        }
    }
}

