using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<Expense> AddExpenseAsync(int tripId, Expense dto, int userId);
        Task<IEnumerable<Expense>> GetExpensesAsync(int tripId, int userId);
        Task<Expense> UpdateExpenseAsync(int tripId, int id, Expense dto, int userId);
        Task<bool> DeleteExpenseAsync(int tripId, int id, int userId);
    }
}
