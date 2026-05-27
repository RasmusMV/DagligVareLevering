using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class NotificationsModel : PageModel
{
    // Repository til at hente notifikationer fra databasen
    private readonly IService<Notification> _notificationRepository;

    public NotificationsModel(IService<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    // Indeholder de notifikationer, der skal vises for den indloggede bruger
    public List<Notification> Notifications { get; set; }
        = new List<Notification>();

    public async Task<IActionResult> OnGetAsync()
    {
        // Henter den indloggede brugers id fra sessionen
        int? userId = HttpContext.Session.GetInt32("UserId");

        // Hvis brugeren ikke er logget ind, sendes brugeren til login
        if (userId == null)
        {
            return RedirectToPage("/UserRelated/Login");
        }

        // Henter kun notifikationer, der tilhører den indloggede bruger
        Notifications =
            (await _notificationRepository.GetObjectsAsync())
            .Where(n => n.UserId == userId.Value)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();

        return Page();
    }
}