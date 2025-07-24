using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Models.Enums;
using BusinessAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool CanView, bool CanEdit)> GetAccessAsync(int tripId, int userId)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null) return (false, false);

            if (trip.UserId == userId) return (true, true);

            var share = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);

            if (share == null) return (false, false);

            return (true, share.AccessLevel == AccessLevel.Edit);
        }

        public async Task<Expense> AddExpenseAsync(int tripId, int userId, Expense expense)
        {
            var (_, canEdit) = await GetAccessAsync(tripId, userId);
            if (!canEdit) throw new UnauthorizedAccessException("User does not have edit access.");

            expense.TripId = tripId;
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<(IEnumerable<Expense> Expenses, IEnumerable<object> Summary, decimal Total)> GetExpensesAsync(int tripId, int userId)
        {
            var (canView, _) = await GetAccessAsync(tripId, userId);
            if (!canView) throw new UnauthorizedAccessException("User does not have view access.");

            var expenses = await _context.Expenses
                .Where(e => e.TripId == tripId)
                .ToListAsync();

            var summary = expenses
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .ToList();

            var total = expenses.Sum(e => e.Amount);

            return (expenses, summary, total);
        }

        public async Task<Expense> UpdateExpenseAsync(int tripId, int userId, int expenseId, Expense updatedExpense)
        {
            var existingExpense = await _context.Expenses.FindAsync(expenseId);
            if (existingExpense == null || existingExpense.TripId != tripId)
                throw new KeyNotFoundException("Expense not found");

            var (_, canEdit) = await GetAccessAsync(tripId, userId);
            if (!canEdit) throw new UnauthorizedAccessException("User does not have edit access.");

            existingExpense.Category = updatedExpense.Category;
            existingExpense.Description = updatedExpense.Description;
            existingExpense.Amount = updatedExpense.Amount;
            existingExpense.Date = updatedExpense.Date;

            await _context.SaveChangesAsync();
            return existingExpense;
        }

        public async Task<bool> DeleteExpenseAsync(int tripId, int userId, int expenseId)
        {
            var expense = await _context.Expenses.FindAsync(expenseId);
            if (expense == null || expense.TripId != tripId)
                return false;

            var (_, canEdit) = await GetAccessAsync(tripId, userId);
            if (!canEdit) throw new UnauthorizedAccessException("User does not have edit access.");

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<object> Expenses, IEnumerable<object> Summary, decimal Total)> GetSharedExpensesAsync(int tripId, int userId)
        {
            var share = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);

            if (share == null)
                throw new UnauthorizedAccessException("User does not have access to shared trip.");

            var expenses = await _context.Expenses
                .Where(e => e.TripId == tripId)
                .ToListAsync();

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

            return (expenseDtos, summary, total);
        }
    }
}
