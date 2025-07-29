using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Implementations
{
    public class AccessService : IAccessService
    {
        private readonly IAccessRepository _repo;

        public AccessService(IAccessRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> HasAccessToTripAsync(int tripId, int userId)
        {
            if (await _repo.IsOwnerAsync(tripId, userId))
                return true;

            return await _repo.IsSharedAsync(tripId, userId);
        }

        public async Task<string> GetAccessLevelAsync(int tripId, int userId)
        {
            // Owner can edit
            if (await _repo.IsOwnerAsync(tripId, userId))
                return "Edit";

            // Shared users only view
            if (await _repo.IsSharedAsync(tripId, userId))
                return "View";

            // No access
            return "None";
        }
    }
}
