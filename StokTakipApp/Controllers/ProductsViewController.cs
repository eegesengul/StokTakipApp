using Microsoft.AspNetCore.Mvc;

namespace StokTakipApp.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Create() => View();
        public IActionResult Edit(int id) => View();
        public IActionResult Delete(int id) => View();
    }
}
