using DagligVareLevering.Models;
using DagligVareLevering.Models.DTOs;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DagligVareLevering.Pages.Product
{
    public class CreateProductModel : PageModel
    {
        private IProductService _productService;
        private IRepository<Models.Store> _storeService;

        public CreateProductModel(IProductService productService, IRepository<Models.Store> storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }

        [BindProperty]
        public ProductDto ProductDto { get; set; }

        public List<Models.Store> StoreList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToPage("/Index");
            }

            await LoadStoresAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadStoresAsync();
                return Page();
            }

            await _productService.CreateProductAsync(ProductDto);
            return RedirectToPage("/Index");
        }

        private async Task LoadStoresAsync()
        {
            StoreList = (await _storeService.GetObjectsAsync()).ToList();
        }
    }
}
