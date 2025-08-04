using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(
            IReviewService reviewService,
            ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        // Version 1.0
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SubmitReviewV1([FromBody] ReviewDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized();

            var success = await _reviewService.SubmitReviewAsync(dto, userId);
            if (!success)
                return BadRequest("Either no access to trip or you already reviewed this trip.");

            return Ok();
        }

        // Version 2.0 with possible extended logic
        [HttpPost]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> SubmitReviewV2([FromBody] ReviewDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized();

            
            var success = await _reviewService.SubmitReviewAsync(dto, userId);
            if (!success)
                return BadRequest(new { message = "Trip access denied or review already exists" });

            return Ok(new { message = "Review submitted successfully (v2)" });
        }

        // Shared for both versions (optional: you can split if needed)
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetReview(int tripId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized();

            var review = await _reviewService.GetReviewAsync(tripId, userId);
            if (review == null)
                return Forbid();

            return Ok(review);
        }
    }
}
