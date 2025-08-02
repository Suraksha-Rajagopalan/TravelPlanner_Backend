using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class UserRepository : GenericRepository<UserModel>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AdminUserDto>> GetAllAdminUserDtosAsync()
        {
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);

            return await _context.Users
                .Include(u => u.Trips)
                .Select(u => new AdminUserDto
                {
                    Id = u.Id,
                    Name = u.UserName ?? string.Empty,
                    Email = u.Email ?? string.Empty,
                    LastLoginDate = u.LastLoginDate,
                    IsActive = u.LastLoginDate > sixMonthsAgo,
                    NumberOfTrips = u.Trips != null ? u.Trips.Count : 0,
                    TripTitles = u.Trips != null
                                 ? u.Trips.Where(t => t.Title != null).Select(t => t.Title!).ToList()
                                 : new List<string>(),
                    Role = u.Role ?? "User" // Default or fallback if needed
                })
                .ToListAsync();
        }
    }
}
