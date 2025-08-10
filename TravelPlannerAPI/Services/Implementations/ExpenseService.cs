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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessService _access;

        public ExpenseService(
            IUnitOfWork unitOfWork,
            IAccessService accessService)
        {
            _unitOfWork = unitOfWork;
            _access = accessService;
        }

        public async Task<ExpenseModel?> AddExpenseAsync(int tripId, ExpenseModel dto, int userId)
        {
            if (dto == null)
                return null;

            if (!await _access.HasAccessToTripAsync(tripId, userId))
                return null;

            dto.TripId = tripId;
            dto.UserId = userId;

            await _unitOfWork.Expenses.AddAsync(dto);
            await _unitOfWork.CompleteAsync();

            return dto;
        }

        public async Task<IEnumerable<ExpenseModel?>> GetExpensesAsync(int tripId, int userId)
        {
            if (!await _access.HasAccessToTripAsync(tripId, userId))
                return new List<ExpenseModel?>();

            return await _unitOfWork.Expenses.GetByTripAsync(tripId);
        }

        public async Task<ExpenseModel?> UpdateExpenseAsync(int tripId, int id, ExpenseModel dto, int userId)
        {
            var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
            if (expense == null || expense.TripId != tripId)
                return null;

            if (!await _access.HasAccessToTripAsync(tripId, userId))
                return null;

            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.Category = dto.Category;

            _unitOfWork.Expenses.Update(expense);
            await _unitOfWork.CompleteAsync();

            return expense;
        }

        public async Task<bool> DeleteExpenseAsync(int tripId, int id, int userId)
        {
            var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
            if (expense == null || expense.TripId != tripId)
                return false;

            if (!await _access.HasAccessToTripAsync(tripId, userId))
                return false;

            _unitOfWork.Expenses.Delete(expense);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
