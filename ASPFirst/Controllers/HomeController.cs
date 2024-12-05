using ASPFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;
using System.Xml.Linq;

namespace ASPFirst.Controllers
{
    // атрибут указывает что этот класс не является контроллером
    //[NonController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // Чтобы обратиться контроллеру из веб-браузера, нам надо в адресной строке набрать адрес_сайта/Имя_контроллера/Действие_контроллера.
        // например https://localhost:7252/Home/Index
        // а уже этот метод вывод нужную страницу из папки Views, метод должен быть public чтобы он работал в браузере
        // [Route("homepage")] атрибут Route имеет больший приоритет чем маршрутизация в Program.cs, поэтому при таком изменении Home/Index работать не будет
        public IActionResult Index()
        {
            return View();// в метод можно передавать html страницу которая есть в папке Home - папка страниц контроллера
        }
        [Route("Expiriments")]// теперь чтобы обратиться к контроллеру надо написать https://localhost:7252/Expiriments , а не https://localhost:7252/Home/Expiriments
        [Route("{controller}/{action}")]// можно добавить еще один атрибут маршрутизации, теперь к контроллеру можно обратить так https://localhost:7252/Home/Expiriments
        public IActionResult Expiriments()
        {
            // способы передачи данных из контроллера в представление, можно передавать что хочешь любой тип данных
            ViewData["Message"]="Hello artem";
            ViewBag.Language = "русский";
            var pivo = new string[]{ "хмельнов","жигулевское","брауберг"}; 
            // сюда нельзя передавать несколько аргументов, чтобы передать несколько можно создать анонимный объект или объект класса
            return View(pivo);
        }
        [NonAction]// атрибут который отключает метод контроллера, при обращении к нему из браузера он работать не будет
        public IActionResult Privacy()
        {
            return View();
        }
        [ActionName("Welcome")]
        public async Task/*string*/ DobroPozhalovat()
        {
            Response.ContentType = "text/html;charset=utf-8";
            System.Text.StringBuilder tableBuilder = new("<h2>Request headers</h2><table>");
            foreach (var header in Request.Headers)
            {
                tableBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");
            }
            tableBuilder.Append("</table>");
            await Response.WriteAsync(tableBuilder.ToString());
        }
        // https://localhost:7252/Home/TransferInfoinParams?name=artem&age=18
        // отправка данных в контроллер делается с помощью ?name=artem&age=18. Получение данных через строку запроса

        //в ASP.NET Core нельзя иметь несколько действий контроллера с одинаковыми именами, но разными параметрами.
        //    перегрузки не получиться

        // https://localhost:7252/Home/TransferInfoinParams?name=artem&age=18&weight=72
        //public string TransferInfoinParams(string name, int? age, int weight)
        //{
        //    return $"Name: {name}, age: {age}, weight: {weight}";
        //}

        // https://localhost:7252/Home/TransferInfoinParams?name=artem&age=18
        [Route("{name:minlength(3)}/{age:int}")]// https://localhost:7252/tom/8
        public string TransferInfoinParams(Person person)
        {
            return $"Person. Name: {person.Name}, age: {person.Age}";
        }

        // https://localhost:7252/Home/TransferInfoinParams?people=artem&people=lava&people=huy
        // https://localhost:7252/Home/TransferInfoinParams?people[1]=artem&people[0]=lava&people[2]=huy здесь для каждого элемента устанавливается индекс
        // https://localhost:7252/Home/TransferInfoinParams?[1]=artem&[0]=lava&[2]=huy
        //public string TransferInfoinParams(string[]people)
        //{
        //    return string.Join(", ",people);
        //}

        // получение и вывод хуйни с помощью Request.Query
        //public string TransferInfoinParams()
        //{
        //    // в квадратных скобках пишется параметр, если мы писали ?name=artem то тут выведет artem
        //    return $"name: {Request.Query["name"]}, age: {Request.Query["age"]}";
        //}

        //Первый метод Index имеет атрибут[HttpGet], поэтому данный метод будет обрабатывать только запросы GET.Для упрощения примера в ответ метод будет возвращать
        //    html-код с формой ввода (хотя естественно, для формы html можно было бы определить отдельную html-страницу или представление)
        [HttpGet]
        public async Task InfoFromForm()
        {
            // action в HTML-форме указывает URL-адрес или путь, на который данные формы будут отправлены при её отправке.
            string content = @"<form method='post' action='/Home/PostInfoFromForm'>
                <label>Name:</label><br />
                <input name='name' /><br />
                <label>Age:</label><br />
                <input type='number' name='age' /><br />
                <input type='submit' value='Send' />
            </form>";
            Response.ContentType = "text/html;charset=utf-8";
            await Response.WriteAsync(content);
        }
        //При нажатии на кнопку Send введенные данные будут отправляться на сервер.Поскольку у элемента<form> не задан атрибут action, который устанавливает адрес,
        //    то введенные данные отправляются на тот же адрес(то есть по сути методу с тем же именем - методу Index). Но поскольку у формы установлен атрибут method='post',
        //    то данные будут отправлять в запросе типа POST

        // !!! Чтобы система могла связать параметры метода и данные формы, необходимо, чтобы атрибуты name у полей формы соответствовали названиям параметров. !!!
        [HttpPost]
        //public string InfoFromForm(string name, int age) => $"{name}: {age}";
        //public string InfoFromForm(Person person)=>$"{person.Name} {person.Age}";// в этом случае разный только регистр у параметра name в html и параметра в классе, но все равно работает
        public string PostInfoFromForm()
        {
            return $"{Request.Form["name"]} {Request.Form["age"]}";
        }
        
        public record class Person(string Name, int Age);
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // IActionResult позволяет контроллеру возвращать различные типы результатов в зависимости от требований запроса.
        // Например, это может быть представление (View), JSON, редирект и другие виды ответов.
        public IActionResult actionResult() 
        {
            // есть куча готовых классов которые можно возращать при обращении к контроллеру, например:
            // BadRequestResult() возращает страницу 400 с ошибкой          
            //return new RedirectResult("https://www.youtube.com/");// этот класс переносит на другую страницу
            return RedirectToAction("TransferInfoinParams", "Home", new Person("Artem",18));// выполняет определенный метод(TransferInfoinParams) контроллера(Home) и передает в метод параметры
        }
        public IActionResult GetFile()
        {
            //// Чтобы получить полный физический путь каталога относительно проекта, воспользуемся свойством AppDomain.CurrentDomain.BaseDirectory,
            //// которое возвращает путь к папке, где находится текущее приложение.
            //string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/hello.txt");
            //// Тип файла - content-type
            //string file_type = "text/plain";
            //// Имя файла - необязательно
            //string file_name = "haytext.txt";
            //return PhysicalFile(file_path, file_type, file_name);// здесь изменение имени файла не работает

            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/hello.txt");
            //// отправка массива байтов
            //byte[] mas = System.IO.File.ReadAllBytes(path);
            //// отправка файла через FileStream
            //FileStream fs = new FileStream(path, FileMode.Open);
            //string file_type = "text/plain";
            //string file_name = "hello2.txt";
            //return File(/*mas*/fs, file_type, file_name);// здесь изменение имени файла работает

            // по умолчанию все пути к файлам в данном случае будут сопоставляться с папкой wwwroot
            // то есть тут файл берется по пути "wwwroot/Files/hello.txt"
            return File("Files/hello.txt", "text/plain", "hello4.txt");
        }

        // ПЕРЕОПРЕДЕЛЕНИЕ МЕТОДОВ БАЗОВОГО КЛАССА Controller
        // Метод OnActionExecuting() выполняется при вызове метода контроллера до его непосредственного выполнения.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("****************************************************");
            Console.WriteLine("Ща начнет работу");
            Console.WriteLine($"Controller: {context.Controller.GetType().Name}");
            Console.WriteLine($"Action: {context.ActionDescriptor.DisplayName}");
            Console.WriteLine("****************************************************");
            base.OnActionExecuting(context);
        }

        // Метод OnActionExecuted() выполняется после выполнения метода контроллера.
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("****************************************************");
            Console.WriteLine("Работу закончил");
            Console.WriteLine($"Controller: {context.Controller.GetType().Name}");
            Console.WriteLine($"Action: {context.ActionDescriptor.DisplayName}");
            Console.WriteLine("****************************************************");
            base.OnActionExecuted(context);
        }
        /*Кроме того, методы могут обслуживать разные типы запросов.
         * Для указания типа запроса HTTP нам надо применить к методу один из атрибутов: [HttpGet], [HttpPost], [HttpPut], [HttpPatch], [HttpDelete] и [HttpHead].
         * Если атрибут явным образом не указан, то метод может обрабатывать все типы запросов: GET, POST, PUT, DELETE.*/

        [Route("TestRoute/{id?}")]// параметр id в ссылке необязателен
        public string Test(int? id)
        {
            if (id is not null)
                return $"Параметр id={id}";
            else
                return $"Параметр id неопределен";
        }
    }
}