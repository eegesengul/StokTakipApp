using Microsoft.AspNetCore.Mvc;

namespace StokTakipApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}