using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Generic;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseModel?> AddExpenseAsync(int tripId, ExpenseModel dto, int userId);
        Task<IEnumerable<ExpenseModel?>> GetExpensesAsync(int tripId, int userId);
        Task<ExpenseModel?> UpdateExpenseAsync(int tripId, int id, ExpenseModel dto, int userId);
        Task<bool> DeleteExpenseAsync(int tripId, int id, int userId);
    }
}
