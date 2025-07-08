using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TravelPlannerBusiness.Dtos;
using TravelPlannerBusiness.Models;
using TravelPlannerBusiness.Models.Data;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = "Admin")] // Only admins can access this
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
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

        return Ok(users);
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound("User not found");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return BadRequest("Failed to delete user");

        return NoContent();
    }
}
