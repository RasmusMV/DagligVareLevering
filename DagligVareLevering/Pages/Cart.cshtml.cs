using DagligVareLevering.EFDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DagligVareLevering.Models
{
    public class CartModel : PageModel
    {
        private readonly AppDbContext _context;

        public CartModel(AppDbContext context)
        {
            _context = context;
        }

        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        public void OnGet()
        {
            int userId = 1; // midlertidigt indtil login er lavet

            BasketItems = _context.BasketItems
                .Include(b => b.Product)
                .Where(b => b.UserId == userId)
                .ToList();
        }

        public IActionResult OnPostRemove(int productId)
        {
            int userId = 1;

            BasketItem? itemToRemove = _context.BasketItems
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            if (itemToRemove != null)
            {
                _context.BasketItems.Remove(itemToRemove);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostIncrease(int productId)
        {
            int userId = 1;

            BasketItem? itemToIncrease = _context.BasketItems
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            if (itemToIncrease != null)
            {
                itemToIncrease.Quantity++;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDecrease(int productId)
        {
            int userId = 1;

            BasketItem? itemToDecrease = _context.BasketItems
                .FirstOrDefault(b => b.ProductId == productId && b.UserId == userId);

            if (itemToDecrease != null)
            {
                itemToDecrease.Quantity--;

                if (itemToDecrease.Quantity <= 0)
                {
                    _context.BasketItems.Remove(itemToDecrease);
                }

                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}

