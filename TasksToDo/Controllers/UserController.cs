using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TasksToDo.DAL.iServices;
using TasksToDo.DAL.Services;
using TasksToDo.Models;

namespace TasksToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly iUserService _userService;

        private readonly IConfiguration _configuration;

        public UserController(iUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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
                Name = model.Name,
                Surname = model.Surname,
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
            try
            {
                var user = await _userService.GetUserByEmailAsync(model.Email);

                if (user != null && VerifyPassword(user.PasswordHash, model.Password))
                {
                    // User authenticated successfully, generate and return a JWT token

                    // User authenticated successfully, generate and return the JWT token

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtSettings = _configuration.GetSection("JwtSettings");
                    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            // Add other claims if needed, e.g., user roles or additional information
                        }),
                        Expires = DateTime.UtcNow.AddHours(1), // Set token expiration
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    // Return the token as part of the response
                    return Ok(new { token = tokenString });
                }

                return Unauthorized(new { message = "Invalid email or password." });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Invalidate the token or session on the client side
            // For example, remove the token from the client's storage
            return Ok(new { message = "Logout successful." });
        }

        // Helper method to verify the password
        private bool VerifyPassword(string hashedPassword, string password)
        {
            // Compare the provided password with the hashed password using BCrypt
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


        // Helper method to hash the password
        private string HashPassword(string password)
        {
            // Generate a secure salt and hash the password using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }
    }
}
