using Microsoft.AspNetCore.Mvc;
using MVCtoConsumeAPI.Models;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MVCtoConsumeAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string baseURL = "http://localhost:5026/api/";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //calling WEBP API Get action to populate view

            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("ToDo");
                if (getData.IsSuccessStatusCode)     
                {
                    string results=getData.Content.ReadAsStringAsync().Result;
                    dt = JsonConvert.DeserializeObject<DataTable>(results);
                }
                else
                {
                    Console.WriteLine("Error calling WEB API");
                }
            }
            ViewData.Model = dt;
            return View();
        }

        public async Task<IActionResult> Index2()
        {
            //calling WEBP API Get action to populate view

            IList<TaskEntity> tasks = new List<TaskEntity>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await client.GetAsync("ToDo");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    tasks = JsonConvert.DeserializeObject<IList<TaskEntity>>(results);
                }
                else
                {
                    Console.WriteLine("Error calling WEB API");
                }
            }
            ViewData.Model = tasks;
            return View();
        }

        
        public async Task<ActionResult<string>> AddTask(TaskEntity task)
        {
            TaskEntity obj = new TaskEntity()
            {
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };

            if(task.Description!=null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage getData = await client.PostAsJsonAsync<TaskEntity>("ToDo",obj);
                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");                   
                    }
                    else
                    {
                        Console.WriteLine("Error calling WEB API");
                    }
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}