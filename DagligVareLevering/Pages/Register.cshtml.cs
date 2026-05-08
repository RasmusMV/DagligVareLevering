using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    private readonly IService<User> _userService;

    public RegisterModel(IService<User> userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public User User { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        //Sætter standardrolle til Customer
        User.Role = UserRole.Customer;

        //Tilføjer brugeren via service
        await _userService.AddObjectAsync(User);

        return RedirectToPage("Login");
    }
}