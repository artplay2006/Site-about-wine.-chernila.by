using Microsoft.AspNetCore.Mvc;

namespace ASPFirst.Controllers
{
    public class ChernilaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
