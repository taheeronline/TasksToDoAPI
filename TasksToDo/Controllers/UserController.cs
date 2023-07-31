using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksToDo.DAL.Services;
using TasksToDo.Models;

namespace TasksToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            // Check if the email is already registered
            var existingUser = await _userService.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email is already registered." });
            }

            // Create a new user entity
            var newUser = new User
            {
                Name= model.Name,
                Surname= model.Surname,
                Email = model.Email,
                PasswordHash = HashPassword(model.PasswordHash) // Hash the password before storing it
            };

            // Add the user to the database
            await _userService.AddUserAsync(newUser);

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.GetUserByEmailAsync(model.Email);

            if (user != null && VerifyPassword(user.PasswordHash, model.Password))
            {
                // User authenticated successfully, generate and return a JWT token

                // Implement JWT token generation and return the token here
                // For simplicity, this example does not include the JWT token generation logic.

                return Ok(new { token = "Your_JWT_Token" });
            }

            return Unauthorized(new { message = "Invalid email or password." });
        }

        // Helper method to verify the password
        private bool VerifyPassword(string hashedPassword, string password)
        {
            // Implement your password verification logic here, e.g., compare hashedPassword with the provided password
            // For simplicity, this example assumes the password is not hashed and compares it directly.
            return hashedPassword == password;
        }


        // Helper method to hash the password
        private string HashPassword(string password)
        {
            // Implement your password hashing logic here, e.g., using BCrypt or PBKDF2
            // For simplicity, this example does not include actual hashing logic.
            return password;
        }
    }
}
