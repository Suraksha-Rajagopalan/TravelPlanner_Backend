using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BusinessAPI.Dtos;
using BusinessAPI.DTOs;
using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Models.Enums;
using BusinessAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace BusinessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TripController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccessService _accessService;

        public TripController(ApplicationDbContext context, IAccessService accessService)
        {
            _context = context;
            _accessService = accessService;
        }

        // GET: api/Trip
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
        {
            var userId = GetUserId();
            var trips = await _context.Trips
                .Where(t => t.UserId == userId)
                .Include(t => t.BudgetDetails)
                .ToListAsync();

            return trips;
        }

        // GET: api/Trip/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTripById(int id)
        {
            var userId = GetUserId();

            if (!await _accessService.HasAccessToTripAsync(id, userId))
                return Forbid("You do not have access to this trip.");

            var trip = await _context.Trips
                .Include(t => t.BudgetDetails)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
                return NotFound();

            return trip;
        }

        // POST: api/Trip
        [HttpPost]
        public async Task<ActionResult<Trip>> CreateTrip([FromBody] TripCreateDto dto)
        {
            var userId = GetUserId();

            var trip = new Trip
            {
                Title = dto.Title,
                Destination = dto.Destination,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Budget = dto.Budget,
                TravelMode = dto.TravelMode,
                Notes = dto.Notes,
                UserId = userId,
                Image = dto.Image,
                Description = dto.Description,
                Duration = dto.Duration,
                BestTime = dto.BestTime,
                Essentials = dto.Essentials ?? new List<string>(),
                TouristSpots = dto.TouristSpots ?? new List<string>()
            };

            if (dto.BudgetDetails != null)
            {
                trip.BudgetDetails = new BudgetDetails
                {
                    Food = dto.BudgetDetails.Food,
                    Hotel = dto.BudgetDetails.Hotel
                };
            }

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTripById), new { id = trip.Id }, trip);
        }

        // PUT: api/Trip/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] TripUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Trip ID mismatch.");

            var userId = GetUserId();

            // Verify ownership before update
            if (!await _accessService.HasAccessToTripAsync(id, userId))
                return Forbid("You do not have access to update this trip.");

            var trip = await _context.Trips
                .Include(t => t.BudgetDetails)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
                return NotFound("Trip not found.");

            // Only owners can update trips here, if shared users allowed, can add more logic.
            if (trip.UserId != userId)
                return Forbid("Only owner can update this trip.");

            // Update trip details
            trip.Title = dto.Title;
            trip.Destination = dto.Destination;
            trip.StartDate = dto.StartDate;
            trip.EndDate = dto.EndDate;
            trip.Description = dto.Description;
            trip.TravelMode = dto.TravelMode;
            trip.Budget = dto.Budget;
            trip.Notes = dto.Notes;
            trip.Image = dto.Image;
            trip.Duration = dto.Duration;
            trip.BestTime = dto.BestTime;
            trip.Essentials = dto.Essentials ?? new List<string>();
            trip.TouristSpots = dto.TouristSpots ?? new List<string>();

            if (dto.BudgetDetails != null)
            {
                if (trip.BudgetDetails == null)
                    trip.BudgetDetails = new BudgetDetails();

                trip.BudgetDetails.Food = dto.BudgetDetails.Food;
                trip.BudgetDetails.Hotel = dto.BudgetDetails.Hotel;
            }
            else if (trip.BudgetDetails != null)
            {
                _context.BudgetDetails.Remove(trip.BudgetDetails);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Trip/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var userId = GetUserId();

            // Check access (ownership only)
            var trip = await _context.Trips
                .Include(t => t.BudgetDetails)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (trip == null)
                return NotFound("Trip not found or unauthorized.");

            if (trip.BudgetDetails != null)
                _context.BudgetDetails.Remove(trip.BudgetDetails);

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Trip/shared/5
        [HttpPut("shared/{id}")]
        public async Task<IActionResult> UpdateSharedTrip(int id, [FromBody] TripUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Trip ID mismatch.");

            var userId = GetUserId();

            // Check if user has access to shared trip
            if (!await _accessService.HasAccessToTripAsync(id, userId))
                return Forbid("You do not have access to this shared trip.");

            var trip = await _context.Trips
                .Include(t => t.BudgetDetails)
                .Include(t => t.SharedUsers)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
                return NotFound("Trip not found.");

            // Check if user has Edit access (you can implement your own logic here)
            var sharedEntry = trip.SharedUsers
                .FirstOrDefault(s => s.SharedWithUserId == userId && s.AccessLevel == AccessLevel.Edit);

            if (sharedEntry == null)
                return Forbid("You do not have edit access to this shared trip.");

            // Update trip details
            trip.Title = dto.Title;
            trip.Destination = dto.Destination;
            trip.StartDate = dto.StartDate;
            trip.EndDate = dto.EndDate;
            trip.Description = dto.Description;
            trip.TravelMode = dto.TravelMode;
            trip.Budget = dto.Budget;
            trip.Notes = dto.Notes;
            trip.Image = dto.Image;
            trip.Duration = dto.Duration;
            trip.BestTime = dto.BestTime;
            trip.Essentials = dto.Essentials ?? new List<string>();
            trip.TouristSpots = dto.TouristSpots ?? new List<string>();

            if (dto.BudgetDetails != null)
            {
                if (trip.BudgetDetails == null)
                    trip.BudgetDetails = new BudgetDetails();

                trip.BudgetDetails.Food = dto.BudgetDetails.Food;
                trip.BudgetDetails.Hotel = dto.BudgetDetails.Hotel;
            }
            else if (trip.BudgetDetails != null)
            {
                _context.BudgetDetails.Remove(trip.BudgetDetails);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Trip/5/review
        [HttpPost("{tripId}/review")]
        public async Task<IActionResult> SubmitReview(int tripId, [FromBody] ReviewDto dto)
        {
            var userId = GetUserId();

            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == tripId);
            if (trip == null)
                return NotFound("Trip not found.");

            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.TripId == tripId && r.UserId == userId);

            if (existingReview != null)
            {
                existingReview.Rating = dto.Rating;
                existingReview.ReviewText = dto.Review;
            }
            else
            {
                var review = new Review
                {
                    TripId = tripId,
                    UserId = userId,
                    Rating = dto.Rating,
                    ReviewText = dto.Review
                };
                _context.Reviews.Add(review);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: api/Trip/5/reviews
        [HttpGet("{tripId}/reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetTripReviews(int tripId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.TripId == tripId)
                .ToListAsync();

            return reviews;
        }

        // Helper method to get logged-in user ID from JWT token
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid user ID.");

            return userId;
        }
    }
}
