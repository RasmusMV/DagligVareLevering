using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace DagligVareLevering.Pages
{
    public class CartModel : PageModel
    {
        private static List<CartItem> _cartItems = new List<CartItem>
        {
            new CartItem
            {
                Product = new Product { Id = 1, Name = "Milk", Price = 11.0 },
                Quantity = 2
            },
            new CartItem
            {
                Product = new Product { Id = 2, Name = "Bread", Price = 15.0 },
                Quantity = 1
            }
        };

        public List<CartItem> CartItems { get; set; }

        public void OnGet()
        {
            CartItems = _cartItems;
        }

        public IActionResult OnPostRemove(int productId)
        {
            CartItem itemToRemove = _cartItems.FirstOrDefault(item => item.Product.Id == productId);

            if (itemToRemove != null)
            {
                _cartItems.Remove(itemToRemove);
            }

            CartItems = _cartItems;
            return Page();
        }

        public IActionResult OnPostIncrease(int productId)
        {
            CartItem itemToIncrease = _cartItems.FirstOrDefault(item => item.Product.Id == productId);

            if (itemToIncrease != null)
            {
                itemToIncrease.Quantity++;
            }

            CartItems = _cartItems;
            return Page();
        }

        public IActionResult OnPostDecrease(int productId)
        {
            CartItem itemToDecrease = _cartItems.FirstOrDefault(item => item.Product.Id == productId);

            if (itemToDecrease != null)
            {
                itemToDecrease.Quantity--;

                if (itemToDecrease.Quantity <= 0)
                {
                    _cartItems.Remove(itemToDecrease);
                }
            }

            CartItems = _cartItems;
            return Page();
        }
    }
}
