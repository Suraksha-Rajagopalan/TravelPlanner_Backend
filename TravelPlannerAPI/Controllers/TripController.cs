using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPlannerAPI.DTOs;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Require authentication for all actions in this controller
    public class TripController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TripController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Trip
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
        {
            var userId = GetUserId();
            var trips = await _context.Trips
                .Where(t => t.UserId == userId) // only return trips of logged-in user
                .Include(t => t.BudgetDetails)
                .ToListAsync();

            return trips;
        }

        // GET: api/Trip/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTripById(int id)
        {
            var userId = GetUserId();

            var trip = await _context.Trips
                .Include(t => t.BudgetDetails)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

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

            var trip = await _context.Trips
                .Include(t => t.BudgetDetails)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (trip == null)
                return NotFound("Trip not found or unauthorized.");

            // Update properties (do NOT update UserId from dto)
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

        // Helper: Extract logged-in user's ID from JWT token
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid user ID.");

            return userId;
        }
    }
}
