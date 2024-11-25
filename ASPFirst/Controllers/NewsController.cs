using Microsoft.AspNetCore.Mvc;

namespace ASPFirst.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
