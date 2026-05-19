using DagligVareLevering.EFDbContext;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class LoginModel : PageModel
{
    // Service bruges til at håndtere login-logikken
    private readonly IUserService _userService;

    public LoginModel(IUserService userService)
    {
        _userService = userService;
    }

    // Binder email-feltet fra loginformularen
    [BindProperty]
    public required string Email { get; set; }

    // Binder password-feltet fra loginformularen
    [BindProperty]
    public required string Password { get; set; }

    // Bruges til at vise fejlbesked, hvis login fejler
    public string ErrorMessage { get; set; } = string.Empty;

    // Viser login-siden
    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        // Forsøger at finde en bruger med den indtastede email og password
        var user = await _userService.LoginAsync(Email, Password);

        if (user != null)
        {
            // Gemmer brugerens id, rolle og navn i sessionen, så andre sider kan se hvem der er logget ind
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetString("UserName", user.Name);
            return RedirectToPage("/Index");
        }

        // Hvis login fejler, vises en fejlbesked på siden
        ErrorMessage = "Invalid login";
        return Page();
    }
}