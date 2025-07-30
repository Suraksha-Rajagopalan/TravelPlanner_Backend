using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Helpers;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {
        private readonly ApplicationDbContext _context;

        public TripRepository(
            ApplicationDbContext context,
            IGenericRepository<Trip> genericRepo
        ) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trip>> GetByUserAsync(int userId)
        {
            return await _context.Trips
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<Trip> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Trips
                .Include(t => t.Reviews)
                .Include(t => t.SharedUsers)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

    public async Task<PaginatedResult<Trip>> GetPaginatedTripsAsync(PaginationParamsDto pagination)
        {
            var query = _context.Trips.AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResult<Trip>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }

    }
}
