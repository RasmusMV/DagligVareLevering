using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
public class AdminModel : PageModel
{
    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Admin")
            return RedirectToPage("/Login");

        return Page();
    }
}