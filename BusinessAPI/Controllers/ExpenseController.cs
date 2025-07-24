using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace BusinessAPI.Controllers
{
    [Route("api/trips/{tripId}/expenses")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
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

        [HttpPost]
        public async Task<IActionResult> AddExpense(int tripId, [FromBody] Expense expense)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var (_, canEdit) = await GetAccess(tripId, userId);
            if (!canEdit) return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            expense.TripId = tripId;
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return Ok(expense);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses(int tripId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var (canView, _) = await GetAccess(tripId, userId);
            if (!canView) return Forbid();

            var expenses = await _context.Expenses
                .Where(e => e.TripId == tripId)
                .ToListAsync();

            var summary = expenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .ToList();

            var total = expenses.Sum(e => e.Amount);

            return Ok(new { expenses, summary, total });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int tripId, int id, [FromBody] Expense updatedExpense)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var existingExpense = await _context.Expenses.FindAsync(id);
            if (existingExpense == null || existingExpense.TripId != tripId)
                return NotFound();

            var (_, canEdit) = await GetAccess(tripId, userId);
            if (!canEdit) return Forbid();

            // Update fields
            existingExpense.Category = updatedExpense.Category;
            existingExpense.Description = updatedExpense.Description;
            existingExpense.Amount = updatedExpense.Amount;
            existingExpense.Date = updatedExpense.Date;

            await _context.SaveChangesAsync();

            return Ok(existingExpense);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int tripId, int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null || expense.TripId != tripId)
                return NotFound();

            var (_, canEdit) = await GetAccess(tripId, userId);
            if (!canEdit) return Forbid();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("/api/trips/{tripId}/shared-expenses")]
        public async Task<IActionResult> GetSharedExpenses(int tripId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Ensure the trip is shared with this user
            var share = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);

            if (share == null)
                return Forbid();

            var expenses = await _context.Expenses
                .Where(e => e.TripId == tripId)
                .ToListAsync();

            // Convert to lightweight DTOs
            var expenseDtos = expenses.Select(e => new
            {
                e.Id,
                e.TripId,
                e.Category,
                e.Description,
                e.Amount,
                e.Date
            });

            var summary = expenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .ToList();

            var total = expenses.Sum(e => e.Amount);

            return Ok(new { expenses = expenseDtos, summary, total });
        }

    }
}