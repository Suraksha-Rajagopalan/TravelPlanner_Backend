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
    public class ChecklistRepository
        : GenericRepository<ChecklistItem>, IChecklistRepository
    {
        private readonly ApplicationDbContext _context;

        public ChecklistRepository(
            ApplicationDbContext context,
            IGenericRepository<ChecklistItem> genericRepo)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChecklistItem>> GetByTripAndUserAsync(int tripId, int userId)
        {
            return await _context.ChecklistItems
                .Where(i => i.TripId == tripId && i.UserId == userId)
                .ToListAsync();
        }
    }
}
