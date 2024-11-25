using Microsoft.AspNetCore.Mvc;

namespace ASPFirst.Controllers
{
    public class BottleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
