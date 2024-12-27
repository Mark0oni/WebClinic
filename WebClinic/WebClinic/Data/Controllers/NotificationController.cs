using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebClinic.Data.Context;
using WebClinic.Data.Models;

public class NotificationController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public NotificationController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> GetNotifications()
    {
        var userId = _userManager.GetUserId(User);
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        if (!notifications.Any())
        {
            return Content("<p>Нет новых уведомлений</p>", "text/html");
        }

        var html = string.Join("", notifications.Select(n => $"<p>@Html.Encode(n.Message)</p>"));

        ViewBag.NotificationCount = await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);

        return Content(html, "text/html");
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead()
    {
        var userId = _userManager.GetUserId(User);
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = _userManager.GetUserId(User);
        var count = await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
        return Json(count);
    }

}
