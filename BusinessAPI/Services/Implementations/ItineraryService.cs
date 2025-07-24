using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Implementations
{
    public class ItineraryService : IItineraryService
    {
        private readonly ApplicationDbContext _context;

        public ItineraryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItineraryItem>> GetItineraryAsync(int tripId)
        {
            return await _context.ItineraryItems
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.ScheduledDateTime)
                .ToListAsync();
        }

        public async Task<ItineraryItem> AddItineraryItemAsync(int tripId, ItineraryItemCreateDto dto)
        {
            var item = new ItineraryItem
            {
                TripId = tripId,
                Title = dto.Title,
                Description = dto.Description,
                ScheduledDateTime = dto.ScheduledDateTime
            };

            _context.ItineraryItems.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<bool> UpdateItineraryItemAsync(ItineraryItem item)
        {
            var exists = await _context.ItineraryItems.AnyAsync(i => i.Id == item.Id);
            if (!exists) return false;

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItineraryItemAsync(int id)
        {
            var item = await _context.ItineraryItems.FindAsync(id);
            if (item == null) return false;

            _context.ItineraryItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ItineraryItem>> GetSharedItineraryAsync(int tripId, int userId)
        {
            var trip = await _context.Trips
                .Include(t => t.SharedUsers)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null) return null;

            var isSharedWithUser = trip.SharedUsers
                .Any(ts => ts.SharedWithUserId == userId);

            if (!isSharedWithUser) return null;

            return await _context.ItineraryItems
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.ScheduledDateTime)
                .ToListAsync();
        }
    }
}
