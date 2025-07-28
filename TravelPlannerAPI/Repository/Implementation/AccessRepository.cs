using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Implementations
{
    public class AccessRepository : IAccessRepository
    {
        private readonly ApplicationDbContext _context;

        public AccessRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsOwnerAsync(int tripId, int userId)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            return trip != null && trip.UserId == userId;
        }

        public Task<bool> IsSharedAsync(int tripId, int userId)
        {
            return _context.TripShares
                .AnyAsync(ts => ts.TripId == tripId && ts.SharedWithUserId == userId);
        }
    }
}
