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
        : GenericRepository<TripShareModel>, ITripShareRepository
    {
        private readonly ApplicationDbContext _context;

        public TripShareRepository(
            ApplicationDbContext context,
            IGenericRepository<TripShareModel> genericRepo
        ) : base(context)
        {
            _context = context;
        }

        public async Task<TripShareModel?> GetByTripAndUserAsync(int tripId, int sharedWithUserId)
        {
            return await _context.TripShares
                .FirstOrDefaultAsync(s
                    => s.TripId == tripId
                    && s.SharedWithUserId == sharedWithUserId);
        }

        public async Task<TripModel?> GetOwnedTripAsync(int tripId, int ownerId)
        {
            return await _context.Trips
                .FirstOrDefaultAsync(t
                    => t.Id == tripId
                    && t.UserId == ownerId);
        }

        public async Task<List<TripShareModel>> GetSharesForUserAsync(int sharedWithUserId)
        {
            return await _context.TripShares
                .Where(s => s.SharedWithUserId == sharedWithUserId)
                .Include(s => s.Trip)
                .ToListAsync();
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task RemoveByUserIdAsync(int userId)
        {
            var tripShares = await _context.TripShares
                .Where(ts => ts.OwnerId == userId || ts.SharedWithUserId == userId)
                .ToListAsync();

            _context.TripShares.RemoveRange(tripShares);
        }



    }
}
