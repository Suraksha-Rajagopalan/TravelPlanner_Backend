using BusinessAPI.Dtos;
using BusinessAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitReview([FromBody] ReviewDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized();

            var success = await _reviewService.SubmitReviewAsync(dto, userId);
            if (!success)
                return BadRequest("Either no access to trip or you already reviewed this trip.");

            return Ok();
        }

        [HttpGet("{tripId}")]
        [Authorize]
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
