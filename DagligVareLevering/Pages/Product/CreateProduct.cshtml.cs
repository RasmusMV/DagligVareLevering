using DagligVareLevering.Models;
using DagligVareLevering.Models.DTOs;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DagligVareLevering.Pages.Product
{
    public class CreateProductModel : PageModel
    {
        private IService<Models.Product> _productService;
        private IService<Models.Store> _storeService;

        public CreateProductModel(IService<Models.Product> productService, IService<Models.Store> storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }

        [BindProperty]
        public ProductDto ProductDto { get; set; }

        public List<Models.Store> StoreList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
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

            var product = new Models.Product
            {
                Name = ProductDto.Name,
                Price = ProductDto.Price,
                Information = ProductDto.Information,
                StoreId = ProductDto.StoreId
            };

            await _productService.AddObjectAsync(product);
            return RedirectToPage("/Index");
        }

        private async Task LoadStoresAsync()
        {
            StoreList = (await _storeService.GetObjectsAsync()).ToList();
        }
    }
}
