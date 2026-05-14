using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class CreateStoreModel : PageModel
    {
        private IRepository<Models.Store> _dbService;

        public CreateStoreModel(IRepository<Models.Store> dbService)
        {
            _dbService = dbService;
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
            await _dbService.AddObjectAsync(Store);
            return RedirectToPage("GetAllStores");
        }
    }
}
