using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Product
{
    public class EditProductModel : PageModel
    {
        private IService<Models.Product> _productService;
        private IService<Models.Store> _storeService;

        public EditProductModel(IService<Models.Product> productService, IService<Models.Store> storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }

        [BindProperty]
        public Models.Product Product { get; set; }

        [BindProperty]
        public string StoreName { get; set; }

        public List<Models.Store> StoreList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToPage("/Index");
            }

            Product = await _productService.GetObjectByIdAsync(id);
            if (Product == null)
            {
                return RedirectToPage("/Index");
            }
            await LoadStoresAsync();

            StoreName = StoreList.FirstOrDefault(s => s.StoreId == Product.StoreId)?.Name;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var stores = await _storeService.GetObjectsAsync();
            var matchedStore = stores.FirstOrDefault(s => s.Name == StoreName);

            if (matchedStore == null)
            {
                ModelState.AddModelError("StoreName", "Store blev ikke fundet, valgte du en gyldig store?");
                await LoadStoresAsync();
                return Page();
            }

            ModelState.Remove("Product.StoreId");
            ModelState.Remove("Product.Store");
            Product.StoreId = matchedStore.StoreId;

            if (!ModelState.IsValid)
            {
                await LoadStoresAsync();
                return Page();
            }
            await _productService.UpdateObjectAsync(Product);
            return RedirectToPage("/Index");
        }

        private async Task LoadStoresAsync()
        {
            StoreList = (await _storeService.GetObjectsAsync()).ToList();
        }
    }
}
