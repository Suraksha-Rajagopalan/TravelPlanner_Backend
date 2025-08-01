﻿using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class ChecklistRepository
        : GenericRepository<ChecklistItemModel>, IChecklistRepository
    {
        private readonly ApplicationDbContext _context;

        public ChecklistRepository(
            ApplicationDbContext context,
            IGenericRepository<ChecklistItemModel> genericRepo)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChecklistItemModel?>> GetByTripAndUserAsync(int tripId, int userId)
        {
            return await _context.ChecklistItems
                .Where(i => i.TripId == tripId && i.UserId == userId)
                .ToListAsync();

        }
    }
}
