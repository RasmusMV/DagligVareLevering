using DagligVareLevering.Models.DTOs;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.OrderFlow
{
    public class CartModel : PageModel
    {
        //Service til at håndtere databaseoperationer for produkter og indkøbskurv
        private readonly IBasketItemService _basketItemService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public CartModel(IBasketItemService basketItemService, IOrderService orderService, IUserService userService)
        {
            _basketItemService = basketItemService;
            _orderService = orderService;
            _userService = userService;
        }

        // CartSummary Dto til at gemme leveringsprisen, den totale pris af produkterne den samlede, og BasketItems
        public CartSummary CartSummary { get; set; }

        // OnGet -metoden henter data for indkøbskurven, herunder hvilke varer der er i kurven, og beregner priserne
        public async Task<IActionResult> OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            // Hent varer i kurven for den aktuelle bruger og beregn priser
            CartSummary = await _basketItemService.GetCartSummaryAsync(userId.Value);
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

            await _basketItemService.RemoveItemAsync(userId.Value, productId);
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

            await _basketItemService.IncreaseQuantityAsync(userId.Value, productId);

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

            await _basketItemService.DecreaseQuantityAsync(userId.Value, productId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            var basketItems = await _basketItemService.GetUserBasketItemsAsync(userId.Value);
            if (!basketItems.Any())
            {
                return RedirectToPage();
            }

            var user = await _userService.GetObjectByIdAsync(userId.Value);
            await _orderService.CheckoutAsync(userId.Value, user.Adress, basketItems);

            return RedirectToPage("/OrderFlow/DeliveryTime");
        }

    }
}

