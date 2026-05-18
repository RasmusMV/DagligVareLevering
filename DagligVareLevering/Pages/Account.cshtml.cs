using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class AccountModel : PageModel
{
    // Service bruges til at hente, opdatere og slette brugere
    private readonly IUserService _userService;

    public AccountModel(IUserService userService)
    {
        _userService = userService;
    }

    // Binder formularens inputfelter til User-objektet
    [BindProperty]
    public User User { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        // Henter den indloggede brugers id fra sessionen
        var userId = HttpContext.Session.GetInt32("UserId");

        // Hvis brugeren ikke er logget ind, sendes brugeren til login
        if (userId == null)
            return RedirectToPage("/Login");

        // Henter brugerens nuværende oplysninger, så de kan vises på kontosiden
        User = await _userService.GetObjectByIdAsync(userId.Value);

        return Page();
    }

    //opdater user
    public async Task<IActionResult> OnPostUpdateAsync()
    {
        // Henter den indloggede brugers id fra sessionen
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToPage("/Login");

        // Henter brugeren fra databasen, så de eksisterende oplysninger kan opdateres
        var userInDb = await _userService.GetObjectByIdAsync(userId.Value);

        if (userInDb != null)
        {
            // Opdaterer brugerens oplysninger ud fra formularens input
            userInDb.Name = User.Name;
            userInDb.Email = User.Email;
            userInDb.Adress = User.Adress;
            userInDb.Phonenumber = User.Phonenumber;
            userInDb.Password = User.Password;
            userInDb.WantsOfferEmails = User.WantsOfferEmails;

            await _userService.UpdateObjectAsync(userInDb);
        }

        return RedirectToPage();
    }

    //slet user
    public async Task<IActionResult> OnPostDeleteAsync()
    {
        // Henter den indloggede brugers id fra sessionen
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToPage("/Login");

        // Henter brugeren, der skal slettes
        var user = await _userService.GetObjectByIdAsync(userId.Value);

        if (user != null)
        { 
            // Sletter brugeren fra databasen
            await _userService.DeleteObjectAsync(user);
        }

        //logger useren ud når den er slettet
        HttpContext.Session.Clear();

        return RedirectToPage("/Index");
    }
}