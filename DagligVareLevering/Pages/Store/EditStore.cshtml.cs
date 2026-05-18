using DagligVareLevering.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class EditStoreModel : PageModel
    {
        private readonly IRepository<Models.Store> _storeService;

        public EditStoreModel(IRepository<Models.Store> storeService)
        {
            _storeService = storeService;
        }
        [BindProperty]
        public Models.Store Store { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToPage("/Index");
            }

            Store = await _storeService.GetObjectByIdAsync(id);
            if(Store == null)
            {
                return RedirectToPage("GetAllStores");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _storeService.UpdateObjectAsync(Store);
            return RedirectToPage("GetAllStores");
        }
    }
}
