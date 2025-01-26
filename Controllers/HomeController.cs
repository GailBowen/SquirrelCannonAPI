using Microsoft.AspNetCore.Mvc;

namespace SquirrelCannon.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
