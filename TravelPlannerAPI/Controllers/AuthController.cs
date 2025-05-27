using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class LoginRequest
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }

        public class SignupRequest
        {
            public required string Name { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user != null)
            {
                return Ok(new { message = "Login successful", email = request.Email });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }

        // POST: api/auth/signup
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] SignupRequest request)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email already registered" });
            }

            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password 
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            // Simulate successful signup
            return Ok(new { message = $"User {request.Name} registered successfully!" });
        }
    }
}
