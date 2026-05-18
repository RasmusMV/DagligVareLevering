using DagligVareLevering.EFDbContext;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;

    public LoginModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public required string Email { get; set; }

    [BindProperty]
    public required string Password { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userService.LoginAsync(Email, Password);

        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetString("UserName", user.Name);
            return RedirectToPage("/Index");
        }

        ErrorMessage = "Invalid login";
        return Page();
    }
}