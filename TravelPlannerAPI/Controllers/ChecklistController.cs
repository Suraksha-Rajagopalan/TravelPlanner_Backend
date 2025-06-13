using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;

namespace TravelPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/trips/{tripId}/checklist")]
    public class ChecklistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChecklistController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChecklistItem>>> GetChecklist(int tripId)
        {
            return await _context.ChecklistItems
                .Where(item => item.TripId == tripId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ChecklistItem>> AddItem(int tripId, ChecklistItem item)
        {
            item.TripId = tripId;
            _context.ChecklistItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetChecklist), new { tripId }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, ChecklistItem item)
        {
            if (id != item.Id) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.ChecklistItems.FindAsync(id);
            if (item == null) return NotFound();
            _context.ChecklistItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
