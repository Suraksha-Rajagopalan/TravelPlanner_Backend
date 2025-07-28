using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interfaces;
using TravelPlannerAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        private readonly IGenericRepository<Expense> _genericRepo;

        public ExpenseService(
            IExpenseRepository expenseRepo,
            IGenericRepository<Expense> genericRepo)
        {
            _expenseRepo = expenseRepo;
            _genericRepo = genericRepo;
        }

        public async Task<Expense> AddExpenseAsync(int tripId, Expense dto, int userId)
        {
            dto.TripId = tripId;
            //dto.UserId = userId;

            await _genericRepo.AddAsync(dto);
            await _genericRepo.SaveAsync();

            return dto;
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync(int tripId, int userId)
        {
            // Optionally filter by userId as well
            return await _expenseRepo.GetByTripAsync(tripId);
        }

        public async Task<Expense> UpdateExpenseAsync(int tripId, int id, Expense dto, int userId)
        {
            var expense = await _genericRepo.GetByIdAsync(id);
            if (expense == null || expense.TripId != tripId)
                return null;

            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.Category = dto.Category;

            _genericRepo.Update(expense);
            await _genericRepo.SaveAsync();

            return expense;
        }

        public async Task<bool> DeleteExpenseAsync(int tripId, int id, int userId)
        {
            var expense = await _genericRepo.GetByIdAsync(id);
            if (expense == null || expense.TripId != tripId)
                return false;

            _genericRepo.Delete(expense);
            await _genericRepo.SaveAsync();

            return true;
        }
    }
}
