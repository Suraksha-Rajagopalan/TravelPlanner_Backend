using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface ITripShareRepository : IGenericRepository<TripShare>
    {
        Task<TripShare> GetByTripAndUserAsync(int tripId, int sharedWithUserId);
        Task<Trip> GetOwnedTripAsync(int tripId, int ownerId);
        Task<List<TripShare>> GetSharesForUserAsync(int sharedWithUserId);
    }
}
