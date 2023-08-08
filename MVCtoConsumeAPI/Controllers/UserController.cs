using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCtoConsumeAPI.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MVCtoConsumeAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        public UserController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Set base URL of the API endpoint
            _httpClient.BaseAddress = new Uri("http://localhost:5026/api/User");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("user/login", content);

            if (response.IsSuccessStatusCode)
            {
                // Read the content of the response (which should contain the JWT token)
                var tokenContent = await response.Content.ReadAsStringAsync();

                // Store the token in a session variable (you can use a more appropriate storage method)
                HttpContext.Session.SetString("JwtToken", tokenContent);
                HttpContext.Session.SetString("UserName", "Tahir Hussain");

                var viewModel = new LayoutViewModel();


                ViewData["UserName"] = HttpContext.Session.GetString("UserName");

                // TODO: Process successful login, e.g., set user session, redirect to the main page, etc.
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Call the API's logout endpoint
            var response = await _httpClient.PostAsync("user/logout", null);

            if (response.IsSuccessStatusCode)
            {
                // TODO: Process successful logout, e.g., clear user session, redirect to a page, etc.
                // For this example, we'll redirect to the Home/Index page.
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            // Handle logout failure if needed
            return RedirectToAction("Index", "Home"); // For example
        }


    }
}
