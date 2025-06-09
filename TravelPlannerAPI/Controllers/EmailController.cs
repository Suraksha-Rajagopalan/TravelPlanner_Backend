using Microsoft.AspNetCore.Mvc;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] EmailRequest request)
        {
            await _emailService.SendEmailAsync(
                request.AccessToken,
                request.FromEmail,
                request.ToEmail,
                request.Subject,
                request.Body
            );

            return Ok(new { message = "Email sent successfully!" });
        }
    }

}
