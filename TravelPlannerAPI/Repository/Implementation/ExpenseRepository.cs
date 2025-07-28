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
    public class ExpenseRepository
        : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(
            ApplicationDbContext context,
            IGenericRepository<Expense> genericRepo
        ) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetByTripAsync(int tripId)
        {
            return await _context.Expenses
                .Where(e => e.TripId == tripId)
                .ToListAsync();
        }
    }
}
