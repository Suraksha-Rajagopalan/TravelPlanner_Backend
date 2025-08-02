using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IChecklistRepository : IGenericRepository<ChecklistItemModel>
    {
        Task<IEnumerable<ChecklistItemModel?>> GetByTripAndUserAsync(int tripId, int userId);
    }
}
