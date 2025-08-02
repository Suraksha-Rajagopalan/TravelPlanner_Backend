using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IExpenseRepository : IGenericRepository<ExpenseModel>
    {
        Task<IEnumerable<ExpenseModel?>> GetByTripAsync(int tripId);
    }
}
