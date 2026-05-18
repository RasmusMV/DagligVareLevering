using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages
{
    public class OfferEmailsModel : PageModel
    {
        // Service bruges til at hente og opdatere brugerens oplysninger
        private readonly IUserService _userService;

        public OfferEmailsModel(IUserService userService)
        {
            _userService = userService;
        }

        // Binder checkboxens værdi til denne property
        [BindProperty]
        public bool WantsOfferEmails { get; set; }

        // Besked der vises til brugeren efter opdatering
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                RedirectToPage("/Login");
            }

            // Henter brugeren, så siden kan vise den nuværende email-indstilling
            var user = await _userService.GetObjectByIdAsync(userId.Value);
            if(user != null)
            {
                WantsOfferEmails = user.WantsOfferEmails;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Henter den indloggede brugers id fra sessionen
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
              return RedirectToPage("/Login");
            }

            // Opdaterer om brugeren ønsker at modtage tilbudsmails
            await _userService.UpdateOfferEmailsAsync(userId.Value, WantsOfferEmails);

            // Viser en besked afhængigt af brugerens valg
            Message = WantsOfferEmails
                    ? "Du er nu tilmeldt tilbudsmails."
                    : "Du er ikke tilmeldt tilbudsmails.";

            return Page();
        }
    }
}