using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class UserRepository : GenericRepository<User>, IUserRepository
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
                    Name = u.UserName ?? "",
                    Email = u.Email ?? "",
                    LastLoginDate = u.LastLoginDate,
                    IsActive = u.LastLoginDate == null || u.LastLoginDate > sixMonthsAgo,
                    NumberOfTrips = u.Trips.Count,
                    TripTitles = u.Trips.Select(t => t.Title).ToList(),
                    Role = u.Role
                })
                .ToListAsync();
        }
    }
}
