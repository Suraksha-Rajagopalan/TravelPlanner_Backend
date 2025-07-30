using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class TripShareController : ControllerBase
    {
        private readonly ITripShareService _service;
        private readonly ILogger<TripShareController> _logger;

        public TripShareController(
            ITripShareService service,
            ILogger<TripShareController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("share")]
        public async Task<IActionResult> ShareTrip([FromBody] TripShareRequestDto request)
        {
            var ownerIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(ownerIdStr, out var ownerId))
                return Unauthorized();

            var (ok, errMsg) = await _service.ShareTripAsync(ownerId, request);
            if (!ok)
                return BadRequest(errMsg);

            return Ok("Trip shared successfully.");
        }

        [MapToApiVersion("1.0")]
        [HttpGet("shared-with-me")]
        public async Task<IActionResult> GetTripsSharedWithMe()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var list = await _service.GetTripsSharedWithUserAsync(userId);
            return Ok(list);
        }
    }
}
