using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Models.Enums;
using BusinessAPI.Services.Implementations;
using BusinessAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;


namespace BusinessAPI.Controllers
{
    [ApiController]
    [Route("api/trips/{tripId}/checklists")]
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

            if (trip.UserId == userId)
                return (true, true);

            var share = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);

            if (share == null)
                return (false, false);

            return (true, share.AccessLevel == AccessLevel.Edit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChecklistItemDto>>> GetChecklist(int tripId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized("User ID not found");

            int userId = int.Parse(userIdClaim);
            var (canView, _) = await GetAccess(tripId, userId);
            if (!canView) return Forbid();

            var items = await _context.ChecklistItems
                .Where(item => item.TripId == tripId)
                .Select(item => new ChecklistItemDto
                {
                    Id = item.Id,
                    TripId = item.TripId,
                    Description = item.Text,
                    IsCompleted = item.IsCompleted,
                    UserId = item.UserId
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<ChecklistItemDto>> AddItem(int tripId, ChecklistItemDto itemDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var (_, canEdit) = await GetAccess(tripId, userId);
            if (!canEdit) return Forbid();

            var item = new ChecklistItem
            {
                TripId = tripId,
                Text = itemDto.Description,
                IsCompleted = itemDto.IsCompleted,
                UserId = userId
            };

            _context.ChecklistItems.Add(item);
            await _context.SaveChangesAsync();

            itemDto.Id = item.Id;
            itemDto.UserId = userId;

            return CreatedAtAction(nameof(GetChecklist), new { tripId = tripId }, itemDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int tripId, int id, ChecklistItemUpdateDto updateDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var item = await _context.ChecklistItems.FindAsync(id);
            if (item == null || item.TripId != tripId)
                return NotFound();

            var (_, canEdit) = await GetAccess(tripId, userId);
            if (!canEdit) return Forbid();

            item.Text = updateDto.Description;
            item.IsCompleted = updateDto.IsCompleted;
            //item.UserId = userId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int tripId, int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var item = await _context.ChecklistItems.FindAsync(id);
            if (item == null || item.TripId != tripId)
                return NotFound();

            var (_, canEdit) = await GetAccess(tripId, userId);
            if (!canEdit) return Forbid();

            _context.ChecklistItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Read-only checklist for shared users with view access
        [HttpGet("/api/shared-trips/{tripId}/checklist")]
        public async Task<ActionResult<IEnumerable<ChecklistItemDto>>> GetSharedTripChecklist(int tripId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var share = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);

            if (share == null || share.AccessLevel != AccessLevel.View)
                return Forbid();

            var items = await _context.ChecklistItems
                .Where(item => item.TripId == tripId)
                .Select(item => new ChecklistItemDto
                {
                    Id = item.Id,
                    TripId = item.TripId,
                    Description = item.Text,
                    IsCompleted = item.IsCompleted,
                    UserId = item.UserId
                })
                .ToListAsync();

            return Ok(items);
        }
    }
}
