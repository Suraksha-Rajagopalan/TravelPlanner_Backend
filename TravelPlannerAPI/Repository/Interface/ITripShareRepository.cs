using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface ITripShareRepository : IGenericRepository<TripShareModel>
    {
        Task<TripShareModel?> GetByTripAndUserAsync(int tripId, int sharedWithUserId);
        Task<TripModel?> GetOwnedTripAsync(int tripId, int ownerId);
        Task<List<TripShareModel>> GetSharesForUserAsync(int sharedWithUserId);
        Task<UserModel?> GetUserByEmailAsync(string email);
    }
}
