using Microsoft.AspNetCore.Mvc;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
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
            if (request.Email == "test@gmail.com" && request.Password == "1234")
            {
                return Ok(new { message = "Login successful", email = request.Email });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }

        // POST: api/auth/signup
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] SignupRequest request)
        {
            // Simulate successful signup
            return Ok(new { message = $"User {request.Name} registered successfully!" });
        }
    }
}
