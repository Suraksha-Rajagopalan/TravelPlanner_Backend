using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interfaces
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetByTripAsync(int tripId);
    }
}
