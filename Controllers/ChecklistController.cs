using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Models.Enums;
using TravelPlannerAPI.Models;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/trips/{tripId}/checklist")]
[Authorize]
public class ChecklistController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ChecklistController(ApplicationDbContext context)
    {
        _context = context;
    }

    private async Task<(bool CanView, bool CanEdit)> GetAccess(int tripId, int userId)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip == null) return (false, false);

        if (trip.UserId == userId) return (true, true);

        var share = await _context.TripShares
            .FirstOrDefaultAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);

        if (share == null) return (false, false);

        return (true, share.AccessLevel == AccessLevel.Edit);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChecklistItem>>> GetChecklist(int tripId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var (canView, _) = await GetAccess(tripId, userId);
        if (!canView) return Forbid();

        return await _context.ChecklistItems
            .Where(item => item.TripId == tripId && item.UserId == userId)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<ChecklistItem>> AddItem(int tripId, ChecklistItemDto itemDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var (_, canEdit) = await GetAccess(tripId, userId);
        if (!canEdit) return Forbid();

        if (itemDto.UserId != userId)
            return Unauthorized("You can't add items for another user.");

        var item = new ChecklistItem
        {
            TripId = tripId,
            Text = itemDto.Description,
            IsCompleted = itemDto.Completed,
            UserId = userId
        };

        _context.ChecklistItems.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetChecklist), new { tripId }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, ChecklistItem item)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var existingItem = await _context.ChecklistItems.FindAsync(id);

        if (existingItem == null) return NotFound();

        var (_, canEdit) = await GetAccess(existingItem.TripId, userId);
        if (!canEdit || existingItem.UserId != userId) return Forbid();

        if (id != item.Id || existingItem.TripId != item.TripId) return BadRequest();

        existingItem.Text = item.Text;
        existingItem.IsCompleted = item.IsCompleted;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var item = await _context.ChecklistItems.FindAsync(id);
        if (item == null) return NotFound();

        var (_, canEdit) = await GetAccess(item.TripId, userId);
        if (!canEdit || item.UserId != userId) return Forbid();

        _context.ChecklistItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
