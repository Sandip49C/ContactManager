using Microsoft.AspNetCore.Mvc;
using ContactManager.Services;

namespace ContactManager.Controllers
{
    public class BaseController : Controller
    {
        protected readonly NotificationService _notificationService;

        public BaseController(NotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
    }
}