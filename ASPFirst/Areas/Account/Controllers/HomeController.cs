using Microsoft.AspNetCore.Mvc;

namespace ASPFirst.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        // Для обращения к такому методу Index мы можем использовать три адреса:
        // http://localhost:xxxx/Account,
        // http://localhost:xxxx/Account/Home
        // http://localhost:xxxx/Account/Home/Index
        // в таком случае если здесь определена маршрутизация, то в файле Program.cs маршрутизацию можно не писать
        [Route("{area}")]
        [Route("{area}/{controller}")]
        [Route("{area}/{controller}/{action}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
