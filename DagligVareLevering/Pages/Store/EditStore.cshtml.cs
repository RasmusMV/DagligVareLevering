using DagligVareLevering.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class EditStoreModel : PageModel
    {
        // Repository bruges til at hente og opdatere butikker
        private readonly IRepository<Models.Store> _storeService;

        public EditStoreModel(IRepository<Models.Store> storeService)
        {
            _storeService = storeService;
        }

        // Binder formularens inputfelter til Store-objektet
        [BindProperty]
        public Models.Store Store { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Henter brugerens rolle fra sessionen
            var role = HttpContext.Session.GetString("UserRole");

            // Kun admin må redigere butikker
            if (role != "Admin")
            {
                return RedirectToPage("/Store/GetAllStores");
            }

            // Henter den butik, der skal redigeres
            Store = await _storeService.GetObjectByIdAsync(id);

            // Hvis butikken ikke findes, sendes admin tilbage til butiksoversigten
            if (Store == null)
            {
                return RedirectToPage("/Store/GetAllStores");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Tjekker at formularens data overholder validation-reglerne i Store-modellen
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Gemmer ændringerne på butikken i databasen

            await _storeService.UpdateObjectAsync(Store);
            return RedirectToPage("/Store/GetAllStores");
        }
    }
}
