using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class UsersModel : PageModel
{
    private readonly AppDbContext _context;

    public UsersModel(AppDbContext context)
    {
        _context = context;
    }

    public List<User> Users { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Admin")
            return RedirectToPage("/Login");

        Users = await _context.Users.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostChangeRoleAsync(int userId, UserRole newRole)
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Admin")
            return RedirectToPage("/Login");

        var user = await _context.Users.FindAsync(userId);

        if (user != null)
        {
            user.Role = newRole;
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}