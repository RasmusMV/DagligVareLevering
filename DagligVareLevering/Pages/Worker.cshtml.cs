using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class WorkerModel : PageModel
{
    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Worker")
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }
}