using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPlannerBusiness.Dtos;
using TravelPlannerBusiness.Models;
using TravelPlannerBusiness.Models.Data;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<bool> HasAccess(int tripId, int userId)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null) return false;

            if (trip.UserId == userId) return true;

            return await _context.TripShares
                .AnyAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitReview([FromBody] ReviewDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized();

            if (!await HasAccess(dto.TripId, userId))
                return Forbid("You do not have access to this trip.");

            var existing = await _context.Reviews
                .FirstOrDefaultAsync(r => r.TripId == dto.TripId && r.UserId == userId);

            if (existing != null)
                return BadRequest("You already reviewed this trip.");

            var review = new Review
            {
                TripId = dto.TripId,
                UserId = userId,
                Rating = dto.Rating,
                ReviewText = dto.Review ?? string.Empty
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{tripId}")]
        [Authorize]
        public async Task<IActionResult> GetReview(int tripId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized();

            if (!await HasAccess(tripId, userId))
                return Forbid("You do not have access to this trip.");

            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.TripId == tripId && r.UserId == userId);

            if (review == null) return NotFound();

            var dto = new ReviewDto
            {
                TripId = review.TripId,
                UserId = review.UserId,
                Rating = review.Rating,
                Review = review.ReviewText
            };

            return Ok(dto);
        }
    }
}
