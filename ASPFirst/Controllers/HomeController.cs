using ASPFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics;
using System.Xml.Linq;

namespace ASPFirst.Controllers
{
    // ������� ��������� ��� ���� ����� �� �������� ������������
    //[NonController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // ����� ���������� ����������� �� ���-��������, ��� ���� � �������� ������ ������� �����_�����/���_�����������/��������_�����������.
        // �������� https://localhost:7252/Home/Index
        // � ��� ���� ����� ����� ������ �������� �� ����� Views, ����� ������ ���� public ����� �� ������� � ��������
        // [Route("homepage")] ������� Route ����� ������� ��������� ��� ������������� � Program.cs, ������� ��� ����� ��������� Home/Index �������� �� �����
        public IActionResult Index()
        {
            return View();// � ����� ����� ���������� html �������� ������� ���� � ����� Home - ����� ������� �����������
        }
        [Route("Expiriments")]// ������ ����� ���������� � ����������� ���� �������� https://localhost:7252/Expiriments , � �� https://localhost:7252/Home/Expiriments
        [Route("{controller}/{action}")]// ����� �������� ��� ���� ������� �������������, ������ � ����������� ����� �������� ��� https://localhost:7252/Home/Expiriments
        public IActionResult Expiriments()
        {
            // ������� �������� ������ �� ����������� � �������������, ����� ���������� ��� ������ ����� ��� ������
            ViewData["Message"]="Hello artem";
            ViewBag.Language = "�������";
            var pivo = new string[]{ "��������","�����������","��������"}; 
            // ���� ������ ���������� ��������� ����������, ����� �������� ��������� ����� ������� ��������� ������ ��� ������ ������
            return View(pivo);
        }
        [NonAction]// ������� ������� ��������� ����� �����������, ��� ��������� � ���� �� �������� �� �������� �� �����
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
        // �������� ������ � ���������� �������� � ������� ?name=artem&age=18. ��������� ������ ����� ������ �������

        //� ASP.NET Core ������ ����� ��������� �������� ����������� � ����������� �������, �� ������� �����������.
        //    ���������� �� ����������

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
        // https://localhost:7252/Home/TransferInfoinParams?people[1]=artem&people[0]=lava&people[2]=huy ����� ��� ������� �������� ��������������� ������
        // https://localhost:7252/Home/TransferInfoinParams?[1]=artem&[0]=lava&[2]=huy
        //public string TransferInfoinParams(string[]people)
        //{
        //    return string.Join(", ",people);
        //}

        // ��������� � ����� ����� � ������� Request.Query
        //public string TransferInfoinParams()
        //{
        //    // � ���������� ������� ������� ��������, ���� �� ������ ?name=artem �� ��� ������� artem
        //    return $"name: {Request.Query["name"]}, age: {Request.Query["age"]}";
        //}

        //������ ����� Index ����� �������[HttpGet], ������� ������ ����� ����� ������������ ������ ������� GET.��� ��������� ������� � ����� ����� ����� ����������
        //    html-��� � ������ ����� (���� �����������, ��� ����� html ����� ���� �� ���������� ��������� html-�������� ��� �������������)
        [HttpGet]
        public async Task InfoFromForm()
        {
            // action � HTML-����� ��������� URL-����� ��� ����, �� ������� ������ ����� ����� ���������� ��� � ��������.
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
        //��� ������� �� ������ Send ��������� ������ ����� ������������ �� ������.��������� � ��������<form> �� ����� ������� action, ������� ������������� �����,
        //    �� ��������� ������ ������������ �� ��� �� �����(�� ���� �� ���� ������ � ��� �� ������ - ������ Index). �� ��������� � ����� ���������� ������� method='post',
        //    �� ������ ����� ���������� � ������� ���� POST

        // !!! ����� ������� ����� ������� ��������� ������ � ������ �����, ����������, ����� �������� name � ����� ����� ��������������� ��������� ����������. !!!
        [HttpPost]
        //public string InfoFromForm(string name, int age) => $"{name}: {age}";
        //public string InfoFromForm(Person person)=>$"{person.Name} {person.Age}";// � ���� ������ ������ ������ ������� � ��������� name � html � ��������� � ������, �� ��� ����� ��������
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
        // IActionResult ��������� ����������� ���������� ��������� ���� ����������� � ����������� �� ���������� �������.
        // ��������, ��� ����� ���� ������������� (View), JSON, �������� � ������ ���� �������.
        public IActionResult actionResult() 
        {
            // ���� ���� ������� ������� ������� ����� ��������� ��� ��������� � �����������, ��������:
            // BadRequestResult() ��������� �������� 400 � �������          
            //return new RedirectResult("https://www.youtube.com/");// ���� ����� ��������� �� ������ ��������
            return RedirectToAction("TransferInfoinParams", "Home", new Person("Artem",18));// ��������� ������������ �����(TransferInfoinParams) �����������(Home) � �������� � ����� ���������
        }
        public IActionResult GetFile()
        {
            //// ����� �������� ������ ���������� ���� �������� ������������ �������, ������������� ��������� AppDomain.CurrentDomain.BaseDirectory,
            //// ������� ���������� ���� � �����, ��� ��������� ������� ����������.
            //string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/hello.txt");
            //// ��� ����� - content-type
            //string file_type = "text/plain";
            //// ��� ����� - �������������
            //string file_name = "haytext.txt";
            //return PhysicalFile(file_path, file_type, file_name);// ����� ��������� ����� ����� �� ��������

            //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/hello.txt");
            //// �������� ������� ������
            //byte[] mas = System.IO.File.ReadAllBytes(path);
            //// �������� ����� ����� FileStream
            //FileStream fs = new FileStream(path, FileMode.Open);
            //string file_type = "text/plain";
            //string file_name = "hello2.txt";
            //return File(/*mas*/fs, file_type, file_name);// ����� ��������� ����� ����� ��������

            // �� ��������� ��� ���� � ������ � ������ ������ ����� �������������� � ������ wwwroot
            // �� ���� ��� ���� ������� �� ���� "wwwroot/Files/hello.txt"
            return File("Files/hello.txt", "text/plain", "hello4.txt");
        }

        // ��������������� ������� �������� ������ Controller
        // ����� OnActionExecuting() ����������� ��� ������ ������ ����������� �� ��� ����������������� ����������.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("****************************************************");
            Console.WriteLine("�� ������ ������");
            Console.WriteLine($"Controller: {context.Controller.GetType().Name}");
            Console.WriteLine($"Action: {context.ActionDescriptor.DisplayName}");
            Console.WriteLine("****************************************************");
            base.OnActionExecuting(context);
        }

        // ����� OnActionExecuted() ����������� ����� ���������� ������ �����������.
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("****************************************************");
            Console.WriteLine("������ ��������");
            Console.WriteLine($"Controller: {context.Controller.GetType().Name}");
            Console.WriteLine($"Action: {context.ActionDescriptor.DisplayName}");
            Console.WriteLine("****************************************************");
            base.OnActionExecuted(context);
        }
        /*����� ����, ������ ����� ����������� ������ ���� ��������.
         * ��� �������� ���� ������� HTTP ��� ���� ��������� � ������ ���� �� ���������: [HttpGet], [HttpPost], [HttpPut], [HttpPatch], [HttpDelete] � [HttpHead].
         * ���� ������� ����� ������� �� ������, �� ����� ����� ������������ ��� ���� ��������: GET, POST, PUT, DELETE.*/

        [Route("TestRoute/{id?}")]// �������� id � ������ ������������
        public string Test(int? id)
        {
            if (id is not null)
                return $"�������� id={id}";
            else
                return $"�������� id �����������";
        }
    }
}