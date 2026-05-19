using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    // Service bruges til at oprette nye brugere
    private readonly IUserService _userService;

    public RegisterModel(IUserService userService)
    {
        _userService = userService;
    }

    // Binder formularens inputfelter til User-objektet
    [BindProperty]
    public User User { get; set; }

    // Viser registreringssiden
    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        // Tjekker om formularens data overholder validation-reglerne i User-modellen
        if (!ModelState.IsValid)
            return Page();

        // Opretter brugeren gennem user service
        await _userService.RegisterUserAsync(User);

        // Sender brugeren videre til login efter oprettelse
        return RedirectToPage("/UserRelated/Login");
    }
}