using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TripReviewsController> _logger;

        public TripReviewsController(ApplicationDbContext context, ILogger<TripReviewsController> logger)
        {
            _context = context;
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

            var reviews = await _context.Reviews
                .Include(r => r.Trip)
                .Include(r => r.User)
                .Where(r => EF.Functions.Like(r.Trip.Destination, $"%{destination}%"))
                .Select(r => new TripReviewDto
                {
                    TripId = r.TripId,
                    TripName = r.Trip.Destination,
                    Username = r.User.UserName,
                    Rating = r.Rating,
                    Comment = r.ReviewText
                })
                .ToListAsync();

            _logger.LogInformation("Found {Count} reviews for destination '{Destination}'", reviews.Count, destination);

            return Ok(reviews);
        }
    }
}
