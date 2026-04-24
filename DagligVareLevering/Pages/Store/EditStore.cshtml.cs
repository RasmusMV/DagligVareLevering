using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class EditStoreModel : PageModel
    {
        private IService<Models.Store> _dbService;

        public EditStoreModel(IService<Models.Store> dbService)
        {
            _dbService = dbService;
        }
        [BindProperty]
        public Models.Store Store { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Store = await _dbService.GetObjectByIdAsync(id);
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
            await _dbService.UpdateObjectAsync(Store);
            return RedirectToPage("GetAllStores");
        }
    }
}
