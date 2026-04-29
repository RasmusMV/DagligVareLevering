using DagligVareLevering.EFDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class LoginModel(AppDbContext context) : PageModel
{
    private readonly AppDbContext _context = context;

    [BindProperty]
    public required string Email { get; set; }

    [BindProperty]
    public required string Password { get; set; }

    public required string ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == Email && u.Password == Password);

        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            return RedirectToPage("/Index");
        }

        ErrorMessage = "Invalid login";
        return Page();
    }
}