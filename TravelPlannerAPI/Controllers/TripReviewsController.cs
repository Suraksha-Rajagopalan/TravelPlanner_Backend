using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravelPlannerAPI.Services.Interfaces;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripReviewsController : ControllerBase
    {
        private readonly ITripReviewService _tripReviewService;
        private readonly ILogger<TripReviewsController> _logger;

        public TripReviewsController(ITripReviewService tripReviewService, ILogger<TripReviewsController> logger)
        {
            _tripReviewService = tripReviewService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTripReviews([FromQuery] string destination)
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                return BadRequest("Destination is required.");
            }

            var result = await _tripReviewService.SearchReviewsByDestinationAsync(destination);
            return Ok(result);
        }
    }
}
