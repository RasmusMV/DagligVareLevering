using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class GetAllStoresModel : PageModel
    {
        private IRepository<Models.Store> _dbService;

        public GetAllStoresModel(IRepository<Models.Store> dbService)
        {
            _dbService = dbService;
        }

        public List<Models.Store> Stores { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToPage("/Index");
            }

            Stores = (await _dbService.GetObjectsAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Models.Store store = await _dbService.GetObjectByIdAsync(id);
            await _dbService.DeleteObjectAsync(store);
            return RedirectToPage("GetAllStores");
        }
    }
}
