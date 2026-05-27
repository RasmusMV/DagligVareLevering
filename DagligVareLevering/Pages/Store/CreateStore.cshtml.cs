using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages.Store
{
    public class CreateStoreModel : PageModel
    {
        // Repository bruges til at oprette nye butikker
        private readonly IService<Models.Store> _storeService;

        public CreateStoreModel(IService<Models.Store> storeService)
        {
            _storeService = storeService;
        }

        // Binder formularens inputfelter til Store-objektet
        [BindProperty]
        public Models.Store Store { get; set; }

        public IActionResult OnGet()
        {
            // Henter brugerens rolle fra sessionen
            var role = HttpContext.Session.GetString("UserRole");

            // Kun admin må tilgå siden til oprettelse af butikker
            if (role != "Admin")
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

            // Opretter butikken i databasen
            await _storeService.AddObjectAsync(Store);
            return RedirectToPage("/Store/GetAllStores");
        }
    }
}
