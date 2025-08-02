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
    public class ItineraryRepository : GenericRepository<ItineraryItemsModel>, IItineraryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<ItineraryItemsModel> _generic;


        public ItineraryRepository(
            ApplicationDbContext context,
            IUnitOfWork unitOfWork,
            IGenericRepository<ItineraryItemsModel> generic) : base(context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _generic = generic;
        }

        public async Task<IEnumerable<ItineraryItemsModel>> GetByTripIdAsync(int tripId)
        {
            return await _context.ItineraryItems
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.ScheduledDateTime)
                .ToListAsync();
        }

        public Task<ItineraryItemsModel?> GetByIdAsync(int id)
            => _generic.GetByIdAsync(id);

        public async Task newAddAsync(ItineraryItemsModel item)
        {
            await _generic.AddAsync(item);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(ItineraryItemsModel item)
        {
            _generic.Update(item);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(ItineraryItemsModel item)
        {
            _generic.Delete(item);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ItineraryItems.AnyAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<ItineraryItemsModel>> GetSharedItineraryAsync(int tripId, int userId)
        {
            var trip = await _context.Trips
                .Include(t => t.SharedUsers)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null || !trip.SharedUsers.Any(s => s.SharedWithUserId == userId))
            {
                return Enumerable.Empty<ItineraryItemsModel>();
            }

            return await GetByTripIdAsync(tripId);
        }

    }
}