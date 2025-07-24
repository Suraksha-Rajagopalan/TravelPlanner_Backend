using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);

            var users = await _context.Users
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

            return users;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
    }
}
