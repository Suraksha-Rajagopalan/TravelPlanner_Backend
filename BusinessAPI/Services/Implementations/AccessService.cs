// Services/AccessService.cs
using BusinessAPI.Models.Data;
using BusinessAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AccessService : IAccessService
{
    private readonly ApplicationDbContext _context;

    public AccessService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasAccessToTripAsync(int tripId, int userId)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip == null)
            return false;

        if (trip.UserId == userId)
            return true;

        // Check if trip is shared with this user
        return await _context.TripShares.AnyAsync(ts => ts.TripId == tripId && ts.SharedWithUserId == userId);
    }
}
