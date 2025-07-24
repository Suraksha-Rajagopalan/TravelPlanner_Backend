using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/trips/{tripId}/itinerary")]
public class ItineraryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public ItineraryController(ApplicationDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItineraryItem>>> GetItinerary(int tripId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!await _accessService.HasAccessToTripAsync(tripId, userId))
            return Forbid();

        var items = await _context.ItineraryItems
            .Where(i => i.TripId == tripId)
            .OrderBy(i => i.ScheduledDateTime)
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> AddItineraryItem(int tripId, [FromBody] ItineraryItemCreateDto dto)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!await _accessService.HasAccessToTripAsync(tripId, userId))
            return Forbid();

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

        _context.ItineraryItems.Add(item);
        await _context.SaveChangesAsync();

        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int tripId, int id, ItineraryItem item)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!await _accessService.HasAccessToTripAsync(tripId, userId))
            return Forbid();

        if (id != item.Id || tripId != item.TripId)
            return BadRequest();

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int tripId, int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!await _accessService.HasAccessToTripAsync(tripId, userId))
            return Forbid();

        var item = await _context.ItineraryItems.FindAsync(id);
        if (item == null || item.TripId != tripId) return NotFound();

        _context.ItineraryItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
