using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class CreateStoreModel : PageModel
    {
        private IService<Models.Store> _dbService;

        public CreateStoreModel(IService<Models.Store> dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public Models.Store Store { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _dbService.AddObjectAsync(Store);
            return RedirectToPage("GetAllStores");
        }
    }
}
