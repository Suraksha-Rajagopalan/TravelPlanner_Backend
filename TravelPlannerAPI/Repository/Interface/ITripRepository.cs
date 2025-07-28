using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interfaces
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        Task<IEnumerable<Trip>> GetByUserAsync(int userId);
        Task<Trip> GetByIdWithIncludesAsync(int id);
    }
}
