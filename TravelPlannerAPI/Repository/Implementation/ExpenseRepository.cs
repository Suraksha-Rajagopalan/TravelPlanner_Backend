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
    public class ExpenseRepository
        : GenericRepository<ExpenseModel>, IExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(
            ApplicationDbContext context
        ) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExpenseModel?>> GetByTripAsync(int tripId)
        {
            return await _context.Expenses
                .Where(e => e.TripId == tripId)
                .ToListAsync();
        }
    }
}
