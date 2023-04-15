using Microsoft.AspNetCore.Mvc;
using MVCtoConsumeAPI.Models;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MVCtoConsumeAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Set base URL of the API endpoint
            _httpClient.BaseAddress = new Uri("http://localhost:5026/api/ToDo");
        }

        public async Task<IActionResult> Index()
        {
            //calling WEBP API Get action to populate view

            DataTable dt = new DataTable();

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage getData = await _httpClient.GetAsync("ToDo");
            if (getData.IsSuccessStatusCode)
            {
                string results = getData.Content.ReadAsStringAsync().Result;
                dt = JsonConvert.DeserializeObject<DataTable>(results);
            }
            else
            {
                Console.WriteLine("Error calling WEB API");
            }
            ViewData.Model = dt;
            return View();
        }

        public async Task<ActionResult<string>> AddTask(TaskEntity task)
        {
            TaskEntity _task = new TaskEntity()
            {
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };

            if (task.Description != null)
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage getData = await _httpClient.PostAsJsonAsync<TaskEntity>("ToDo", _task);
                if (getData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine("Error calling WEB API");
                }
            }
            return View();
        }

        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //calling WEBP API Get action to populate view

            TaskEntity task = new TaskEntity();

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage getData = await _httpClient.GetAsync($"ToDo/{id}");
            if (getData.IsSuccessStatusCode)
            {
                string results = getData.Content.ReadAsStringAsync().Result;
                task = JsonConvert.DeserializeObject<TaskEntity>(results);
            }
            else
            {
                Console.WriteLine("Error calling WEB API");
            }
            ViewData.Model = task;

            return View(task);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Send DELETE request to the API endpoint to delete the specified todo item
            var response = await _httpClient.DeleteAsync($"ToDo/{id}");

            // Check if the deletion was successful
            if (response.IsSuccessStatusCode)
            {
                // Return a success response
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Return an error response
                return BadRequest(response.ReasonPhrase);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            //calling WEBP API Get action to populate view

            TaskEntity task = new TaskEntity();

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage getData = await _httpClient.GetAsync($"ToDo/{id}");
            if (getData.IsSuccessStatusCode)
            {
                string results = getData.Content.ReadAsStringAsync().Result;
                task = JsonConvert.DeserializeObject<TaskEntity>(results);
            }
            else
            {
                Console.WriteLine("Error calling WEB API");
            }
            ViewData.Model = task;

            return View(task);
        }

        // POST: ToDoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskEntity task)
        {
            //HTTP POST
            if (task.IsCompleted == false)
            {
                task.CompletedDate = DateTime.Now;
            }
            else
            {
                task.CompletedDate = task.CreatedDate;
            }
            var _task = await _httpClient.PutAsJsonAsync<TaskEntity>("ToDo", task);

            if (_task.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            return View(_task);
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