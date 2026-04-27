using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DagligVareLevering.Pages
{
    public class OfferEmailsModel : PageModel
    {
        private IService<User> _userService;

        [BindProperty]
        public bool WantsOfferEmails { get; set; }

        public string Message { get; set; }

        public OfferEmailsModel(IService<User> userService)
        {
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            var users = await _userService.GetObjectsAsync();
            var user = users.FirstOrDefault();

            if (user != null)
            {
                WantsOfferEmails = user.WantsOfferEmails;
            }
        }

        public async Task OnPostAsync()
        {
            var users = await _userService.GetObjectsAsync();
            var user = users.FirstOrDefault();

            if (user != null)
            {
                user.WantsOfferEmails = WantsOfferEmails;

                await _userService.UpdateObjectAsync(user);

                Message = WantsOfferEmails
                    ? "Du er nu tilmeldt tilbudsmails."
                    : "Du er ikke tilmeldt tilbudsmails.";
            }
            else
            {
                Message = "Der blev ikke fundet en bruger.";
            }
        }
    }
}