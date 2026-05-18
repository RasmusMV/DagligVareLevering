using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class GetAllStoresModel : PageModel
    {
        private readonly IRepository<Models.Store> _storeService;

        public GetAllStoresModel(IRepository<Models.Store> storeService)
        {
            _storeService = storeService;
        }

        public List<Models.Store> Stores { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToPage("/Index");
            }

            Stores = (await _storeService.GetObjectsAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Models.Store store = await _storeService.GetObjectByIdAsync(id);
            await _storeService.DeleteObjectAsync(store);
            return RedirectToPage("GetAllStores");
        }
    }
}
