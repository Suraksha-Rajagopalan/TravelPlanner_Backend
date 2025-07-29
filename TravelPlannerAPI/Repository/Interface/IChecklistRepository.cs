using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IChecklistRepository : IGenericRepository<ChecklistItem>
    {
        Task<IEnumerable<ChecklistItem>> GetByTripAndUserAsync(int tripId, int userId);
    }
}
