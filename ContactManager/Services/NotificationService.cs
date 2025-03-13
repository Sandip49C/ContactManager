using ContactManager.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ContactManager.Services
{
    public class NotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void AddNotification(string message, string type)
        {
            var notifications = GetNotifications();
            notifications.Add(new Notification
            {
                Message = message ?? throw new ArgumentNullException(nameof(message)),
                Type = type ?? throw new ArgumentNullException(nameof(type)),
                CreatedAt = DateTime.Now,
                IsRead = false,
                UserId = 1 // Hardcoded for now; update with actual user ID in a real app
            });
            SaveNotifications(notifications);
        }

        public List<Notification> GetNotifications()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                return new List<Notification>();
            }
            var notificationsJson = session.GetString("Notifications");
            return string.IsNullOrEmpty(notificationsJson)
                ? new List<Notification>()
                : JsonSerializer.Deserialize<List<Notification>>(notificationsJson) ?? new List<Notification>();
        }

        private void SaveNotifications(List<Notification> notifications)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var notificationsJson = JsonSerializer.Serialize(notifications);
                session.SetString("Notifications", notificationsJson);
            }
        }
    }
}