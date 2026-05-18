using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class AccountModel : PageModel
{
    private readonly IUserService _userService;

    public AccountModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public User User { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToPage("/Login");

        User = await _userService.GetObjectByIdAsync(userId.Value);

        return Page();
    }

    //opdater user
    public async Task<IActionResult> OnPostUpdateAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToPage("/Login");

        var userInDb = await _userService.GetObjectByIdAsync(userId.Value);

        if (userInDb != null)
        {
            userInDb.Name = User.Name;
            userInDb.Email = User.Email;
            userInDb.Adress = User.Adress;
            userInDb.Phonenumber = User.Phonenumber;
            userInDb.Password = User.Password;

            await _userService.UpdateObjectAsync(userInDb);
        }

        return RedirectToPage();
    }

    //slet user
    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToPage("/Login");

        var user = await _userService.GetObjectByIdAsync(userId.Value);

        if (user != null)
        {
            await _userService.DeleteObjectAsync(user);
        }

        //logger useren ud når den er slettet
        HttpContext.Session.Clear();

        return RedirectToPage("/Index");
    }
}