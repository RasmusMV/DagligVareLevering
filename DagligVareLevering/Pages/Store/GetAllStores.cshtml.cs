using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class GetAllStoresModel : PageModel
    {
        // Repository bruges til at hente og slette butikker
        private readonly IRepository<Models.Store> _storeService;

        public GetAllStoresModel(IRepository<Models.Store> storeService)
        {
            _storeService = storeService;
        }

        // Indeholder alle butikker, som admin kan se på siden
        public List<Models.Store> Stores { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Henter brugerens rolle fra sessionen
            var role = HttpContext.Session.GetString("UserRole");
           
            // Kun admin må se siden med alle butikker
            if (role != "Admin")
            {
                return RedirectToPage("/Index");
            }

            // Henter alle butikker fra databasen
            Stores = (await _storeService.GetObjectsAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // Finder den butik, der skal slettes
            Models.Store store = await _storeService.GetObjectByIdAsync(id);

            // Sletter butikken fra databasen
            await _storeService.DeleteObjectAsync(store);
            return RedirectToPage("GetAllStores");
        }
    }
}
