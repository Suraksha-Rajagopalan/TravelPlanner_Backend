using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPlannerBusiness.Dtos;
using TravelPlannerBusiness.Models;
using TravelPlannerBusiness.Models.Data;

namespace TravelPlannerAPI.Controllers
{
    [Route("api/trips/{tripId}/itinerary")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItineraryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItineraryItem>>> GetItinerary(int tripId)
        {
            return await _context.ItineraryItems
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.ScheduledDateTime)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddItineraryItem(
    int tripId,
    [FromBody] ItineraryItemCreateDto dto)
        {
            ModelState.Remove("Trip");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = new ItineraryItem
            {
                TripId = tripId,
                Title = dto.Title,
                Description = dto.Description,
                ScheduledDateTime = dto.ScheduledDateTime
            };

            Console.WriteLine($"Title: {item?.Title}, Desc: {item?.Description}, Time: {item?.ScheduledDateTime}");


            _context.ItineraryItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, ItineraryItem item)
        {
            if (id != item.Id) return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.ItineraryItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.ItineraryItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("/api/shared/trips/{tripId}/itinerary")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ItineraryItem>>> GetSharedItinerary(int tripId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var trip = await _context.Trips
                .Include(t => t.SharedUsers)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                return NotFound("Trip not found");

            var isSharedWithUser = trip.SharedUsers
                .Any(ts => ts.SharedWithUserId == userId);

            if (!isSharedWithUser)
                return Forbid();

            return await _context.ItineraryItems
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.ScheduledDateTime)
                .ToListAsync();
        }

    }

}
