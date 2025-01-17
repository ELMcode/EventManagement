using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
