using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class TripShareRepository
        : GenericRepository<TripShare>, ITripShareRepository
    {
        private readonly ApplicationDbContext _context;

        public TripShareRepository(
            ApplicationDbContext context,
            IGenericRepository<TripShare> genericRepo
        ) : base(context)
        {
            _context = context;
        }

        public async Task<TripShare> GetByTripAndUserAsync(int tripId, int sharedWithUserId)
        {
            return await _context.TripShares
                .FirstOrDefaultAsync(s
                    => s.TripId == tripId
                    && s.SharedWithUserId == sharedWithUserId);
        }

        public async Task<Trip> GetOwnedTripAsync(int tripId, int ownerId)
        {
            return await _context.Trips
                .FirstOrDefaultAsync(t
                    => t.Id == tripId
                    && t.UserId == ownerId);
        }

        public async Task<List<TripShare>> GetSharesForUserAsync(int sharedWithUserId)
        {
            return await _context.TripShares
                .Where(s => s.SharedWithUserId == sharedWithUserId)
                .Include(s => s.Trip)
                .ToListAsync();
        }
    }
}
