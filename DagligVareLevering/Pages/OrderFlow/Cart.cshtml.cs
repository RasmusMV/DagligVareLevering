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
        // CartSummary DTO bruges til at samle kurvens varer, leveringspris og totalpris ét sted
        public CartSummary CartSummary { get; set; }

        public async Task<IActionResult> OnGet()
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }

            // Henter kurvens indhold og beregner priser via basket service
            CartSummary = await _basketItemService.GetCartSummaryAsync(userId.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int productId)
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }

            // Fjerner produktet fra brugerens kurv
            await _basketItemService.RemoveItemAsync(userId.Value, productId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostIncreaseAsync(int productId)
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }

            // Øger antallet af det valgte produkt i kurven
            await _basketItemService.IncreaseQuantityAsync(userId.Value, productId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDecreaseAsync(int productId)
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }

            // Sænker antallet af det valgte produkt i kurven
            await _basketItemService.DecreaseQuantityAsync(userId.Value, productId);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/UserRelated/Login");
            }

            // Henter brugerens kurv med tilhørende produkter
            var basketItems = await _basketItemService.GetUserBasketItemsWithProductsAsync(userId.Value);

            // Hvis kurven er tom, bliver brugeren på kurvsiden
            if (!basketItems.Any())
            {
                return RedirectToPage();
            }

            // Henter brugerens adresse, så den kan bruges som standard leveringsadresse
            var user = await _userService.GetObjectByIdAsync(userId.Value);

            // Opretter en ordre ud fra kurvens varer
            await _orderService.CheckoutAsync(userId.Value, user.Adress, basketItems);

            return RedirectToPage("/OrderFlow/DeliveryTime");
        }

    }
}

