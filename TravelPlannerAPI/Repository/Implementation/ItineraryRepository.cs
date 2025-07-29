using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class ItineraryRepository : IItineraryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<ItineraryItem> _generic;
        private readonly IUnitOfWork _unitOfWork;

        public ItineraryRepository(
            ApplicationDbContext context,
            IGenericRepository<ItineraryItem> genericRepository,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _generic = genericRepository;
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(ItineraryItem item)
        {
            _generic.Update(item);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(ItineraryItem item)
        {
            _generic.Delete(item);
            await _unitOfWork.CompleteAsync();
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
