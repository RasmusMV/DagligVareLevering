using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _context;

    public RegisterModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public User User { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        //Sætter standard rolle til Customer
        User.Role = UserRole.Customer;

        //Tilføjer brugeren til databasen
        _context.Users.Add(User);
        await _context.SaveChangesAsync();

        return RedirectToPage("Login");
    }
}