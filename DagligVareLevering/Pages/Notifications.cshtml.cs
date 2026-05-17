using DagligVareLevering.Models;
using DagligVareLevering.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class NotificationsModel : PageModel
{
    private IService<Notification> _notificationService;

    public NotificationsModel(IService<Notification> notificationService)
    {
        _notificationService = notificationService;
    }

    public List<Notification> Notifications { get; set; }
        = new List<Notification>();

    public async Task OnGetAsync()
    {
        int currentUserId =
            HttpContext.Session.GetInt32("UserId").Value;

        Notifications =
            (await _notificationService.GetObjectsAsync())
            .Where(n => n.UserId == currentUserId)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
    }
}