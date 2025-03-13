using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ContactManager.Services;

namespace ContactManager.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(NotificationService notificationService) : base(notificationService)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}