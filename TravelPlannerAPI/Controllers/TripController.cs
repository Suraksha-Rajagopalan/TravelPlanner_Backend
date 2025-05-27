using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TripController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
        {
            return await _context.Trips.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Trip>> CreateTrip(Trip trip)
        {
            // Validate UserId
            var userExists = await _context.Users.AnyAsync(u => u.Id == trip.UserId);
            if (!userExists)
            {
                return BadRequest("Invalid UserId. The user does not exist.");
            }

            // Add trip
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrips), new { id = trip.Id }, trip);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, Trip updatedTrip)
        {
            if (id != updatedTrip.Id)
            {
                return BadRequest("Trip ID mismatch.");
            }

            var trip = await _context.Trips.FindAsync(id);

            if (trip == null)
            {
                return NotFound("Trip not found.");
            }

            // Update properties
            trip.Title = updatedTrip.Title;
            trip.Destination = updatedTrip.Destination;
            trip.StartDate = updatedTrip.StartDate;
            trip.EndDate = updatedTrip.EndDate;
            trip.Description = updatedTrip.Description;
            trip.UserId = updatedTrip.UserId; 

            await _context.SaveChangesAsync();

            return NoContent(); // 204 response
        }

    }
}
