using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class AdminChangesModel : PageModel
{
    // Service bruges til at hente og opdatere brugere
    private readonly IUserService _userService;

    public AdminChangesModel(IUserService userService)
    {
        _userService = userService;
    }

    // Indeholder alle brugere, som admin kan se og administrere
    public List<User> Users { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        // Henter brugerens rolle fra sessionen
        var role = HttpContext.Session.GetString("UserRole");

        // Kun admin må se user management-siden
        if (role != "Admin")
            return RedirectToPage("/UserRelated/Login");

        // Henter alle brugere fra databasen
        Users = (await _userService.GetObjectsAsync()).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostChangeRoleAsync(int userId, UserRole newRole)
    {
        // Henter brugerens rolle fra sessionen
        var role = HttpContext.Session.GetString("UserRole");

        // Kun admin må ændre roller
        if (role != "Admin")
            return RedirectToPage("/UserRelated/Login");

        // Finder den bruger, hvis rolle skal ændres
        var user = await _userService.GetObjectByIdAsync(userId);

        if (user != null)
        {
            // Opdaterer brugerens rolle og gemmer ændringen
            user.Role = newRole;
            await _userService.UpdateObjectAsync(user);
        }

        return RedirectToPage();
    }
}