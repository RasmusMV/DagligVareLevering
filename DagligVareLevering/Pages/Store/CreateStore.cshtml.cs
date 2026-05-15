using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class CreateStoreModel : PageModel
    {
        private readonly IRepository<Models.Store> _storeService;

        public CreateStoreModel(IRepository<Models.Store> storeService)
        {
            _storeService = storeService;
        }

        [BindProperty]
        public Models.Store Store { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if(role != "Admin")
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _storeService.AddObjectAsync(Store);
            return RedirectToPage("GetAllStores");
        }
    }
}
