using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Implementations
{
    public class ItineraryRepository : IItineraryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<ItineraryItem> _generic;

        public ItineraryRepository(
            ApplicationDbContext context,
            IGenericRepository<ItineraryItem> genericRepository)
        {
            _context = context;
            _generic = genericRepository;
        }

        public async Task<IEnumerable<ItineraryItem>> GetByTripIdAsync(int tripId)
        {
            return await _context.ItineraryItems
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.ScheduledDateTime)
                .ToListAsync();
        }

        public Task<ItineraryItem> GetByIdAsync(int id)
            => _generic.GetByIdAsync(id);

        public async Task AddAsync(ItineraryItem item)
        {
            await _generic.AddAsync(item);
            await _generic.SaveAsync();
        }

        public async Task UpdateAsync(ItineraryItem item)
        {
            _generic.Update(item);
            await _generic.SaveAsync();
        }

        public async Task DeleteAsync(ItineraryItem item)
        {
            _generic.Delete(item);
            await _generic.SaveAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ItineraryItems.AnyAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<ItineraryItem>> GetSharedItineraryAsync(int tripId, int userId)
        {
            var trip = await _context.Trips
                .Include(t => t.SharedUsers)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null ||
                !trip.SharedUsers.Any(s => s.SharedWithUserId == userId))
            {
                return null;
            }

            return await GetByTripIdAsync(tripId);
        }
    }
}
