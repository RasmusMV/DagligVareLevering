using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class AccountModel : PageModel
{
    private readonly AppDbContext _context;

    public AccountModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public User User { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToPage("/Login");

        User = await _context.Users.FindAsync(userId);

        return Page();
    }

    //opdater user
    public async Task<IActionResult> OnPostUpdateAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToPage("/Login");

        var userInDb = await _context.Users.FindAsync(userId);

        if (userInDb != null)
        {
            userInDb.Name = User.Name;
            userInDb.Email = User.Email;
            userInDb.Adress = User.Adress;
            userInDb.Phonenumber = User.Phonenumber;
            userInDb.Password = User.Password;

            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    //slet user
    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToPage("/Login");

        var user = await _context.Users.FindAsync(userId);

        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        //logger useren ud når den er slettet
        HttpContext.Session.Clear();

        return RedirectToPage("/Index");
    }
}