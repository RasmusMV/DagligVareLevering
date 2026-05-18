using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages
{
    public class OfferEmailsModel : PageModel
    {
        private readonly IUserService _userService;

        public OfferEmailsModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public bool WantsOfferEmails { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId == null)
            {
                RedirectToPage("/Login");
            }

            var user = await _userService.GetObjectByIdAsync(userId.Value);
            if(user != null)
            {
                WantsOfferEmails = user.WantsOfferEmails;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                RedirectToPage("/Login");
            }

            await _userService.UpdateOfferEmailsAsync(userId.Value, WantsOfferEmails);

            Message = WantsOfferEmails
                    ? "Du er nu tilmeldt tilbudsmails."
                    : "Du er ikke tilmeldt tilbudsmails.";

            return Page();
        }
    }
}