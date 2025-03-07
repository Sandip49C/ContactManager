using ContactManager.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ContactManager.Services
{
    public class NotificationService
    {
        private const string NotificationKey = "Notifications";
        private readonly ISession _session;

        public NotificationService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext?.Session;
        }

        public void AddNotification(string message, string type = "success")
        {
            ClearNotifications();
            var notifications = new List<Notification>();
            notifications.Add(new Notification { Message = message, Type = type });
            _session.SetString(NotificationKey, JsonConvert.SerializeObject(notifications));
        }

        public List<Notification> GetNotifications()
        {
            var json = _session.GetString(NotificationKey);
            return json == null ? new List<Notification>() : JsonConvert.DeserializeObject<List<Notification>>(json);
        }

        public void ClearNotifications()
        {
            _session.Remove(NotificationKey);
        }
    }
}