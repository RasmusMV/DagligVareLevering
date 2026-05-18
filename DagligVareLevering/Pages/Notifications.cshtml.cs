using DagligVareLevering.Models;
using DagligVareLevering.Repositories.Interfaces;
using DagligVareLevering.Service;
using DagligVareLevering.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class NotificationsModel : PageModel
{
    private readonly IRepository<Notification> _notificationRepository;

    public NotificationsModel(IRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public List<Notification> Notifications { get; set; }
        = new List<Notification>();

    public async Task<IActionResult> OnGetAsync()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if(userId == null)
        {
            return RedirectToPage("/Login");
        }

        Notifications =
            (await _notificationRepository.GetObjectsAsync())
            .Where(n => n.UserId == userId.Value)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();

        return Page();
    }
}