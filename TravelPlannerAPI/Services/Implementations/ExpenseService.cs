using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        private readonly IGenericRepository<ExpenseModel> _genericRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseService(
            IExpenseRepository expenseRepo,
            IGenericRepository<ExpenseModel> genericRepo,
            IUnitOfWork unitOfWork)
        {
            _expenseRepo = expenseRepo;
            _genericRepo = genericRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<ExpenseModel?> AddExpenseAsync(int tripId, ExpenseModel dto, int userId)
        {
            if (dto == null)
                return null;

            dto.TripId = tripId;
            //dto.UserId = userId;

            await _genericRepo.AddAsync(dto);
            await _unitOfWork.CompleteAsync();

            return dto;
        }

        public async Task<IEnumerable<ExpenseModel?>> GetExpensesAsync(int tripId, int userId)
        {
            // Optionally filter by userId as well
            return await _expenseRepo.GetByTripAsync(tripId);
        }

        public async Task<ExpenseModel?> UpdateExpenseAsync(int tripId, int id, ExpenseModel dto, int userId)
        {
            var expense = await _genericRepo.GetByIdAsync(id);
            if (expense == null || expense.TripId != tripId)
                return null;

            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.Category = dto.Category;

            _genericRepo.Update(expense);
            await _unitOfWork.CompleteAsync();

            return expense;
        }

        public async Task<bool> DeleteExpenseAsync(int tripId, int id, int userId)
        {
            var expense = await _genericRepo.GetByIdAsync(id);
            if (expense == null || expense.TripId != tripId)
                return false;

            _genericRepo.Delete(expense);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
