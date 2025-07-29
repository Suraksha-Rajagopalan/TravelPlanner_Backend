using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(
            ApplicationDbContext context,
            IGenericRepository<Review> genericRepository)
            : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasAccessAsync(int tripId, int userId)
        {
            // Owner?
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null) return false;
            if (trip.UserId == userId) return true;

            // Shared?
            return await _context.TripShares
                .AnyAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);
        }

        public async Task<Review> GetByTripAndUserAsync(int tripId, int userId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.TripId == tripId && r.UserId == userId);
        }
    }
}
