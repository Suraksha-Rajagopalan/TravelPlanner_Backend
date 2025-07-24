using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace BusinessAPI.Controllers
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
            _logger.LogInformation("SearchTripReviews called with destination: {Destination}", destination);

            if (string.IsNullOrWhiteSpace(destination))
            {
                _logger.LogWarning("Destination is empty.");
                return BadRequest("Destination is required.");
            }

            var reviews = await _tripReviewService.SearchReviewsByDestinationAsync(destination);

            _logger.LogInformation("Found {Count} reviews for destination '{Destination}'", reviews.Count, destination);

            return Ok(reviews);
        }
    }
}
