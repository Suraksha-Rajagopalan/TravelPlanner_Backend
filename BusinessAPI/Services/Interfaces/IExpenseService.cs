using BusinessAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<(bool CanView, bool CanEdit)> GetAccessAsync(int tripId, int userId);
        Task<Expense> AddExpenseAsync(int tripId, int userId, Expense expense);
        Task<(IEnumerable<Expense> Expenses, IEnumerable<object> Summary, decimal Total)> GetExpensesAsync(int tripId, int userId);
        Task<Expense> UpdateExpenseAsync(int tripId, int userId, int expenseId, Expense updatedExpense);
        Task<bool> DeleteExpenseAsync(int tripId, int userId, int expenseId);
        Task<(IEnumerable<object> Expenses, IEnumerable<object> Summary, decimal Total)> GetSharedExpensesAsync(int tripId, int userId);
    }
}
