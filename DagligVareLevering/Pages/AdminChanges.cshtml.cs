using DagligVareLevering.EFDbContext;
using DagligVareLevering.Models;
using DagligVareLevering.Models.Enums;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class UsersModel : PageModel
{
    private readonly IUserService _userService;

    public UsersModel(IUserService userService)
    {
        _userService = userService;
    }

    public List<User> Users { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Admin")
            return RedirectToPage("/Login");

        Users = (await _userService.GetObjectsAsync()).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostChangeRoleAsync(int userId, UserRole newRole)
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Admin")
            return RedirectToPage("/Login");

        var user = await _userService.GetObjectByIdAsync(userId);

        if (user != null)
        {
            user.Role = newRole;
            await _userService.UpdateObjectAsync(user);
        }

        return RedirectToPage();
    }
}