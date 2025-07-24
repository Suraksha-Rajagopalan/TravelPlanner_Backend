using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Models.Enums;
using BusinessAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAPI.Services
{
    public class TripShareService : ITripShareService
    {
        private readonly ApplicationDbContext _context;

        public TripShareService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string ErrorMessage)> ShareTripAsync(int ownerId, TripShareRequestDto request)
        {
            // Lookup user by email
            var sharedWithUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.SharedWithEmail);

            if (sharedWithUser == null)
                return (false, "User to share with not found.");

            if (sharedWithUser.Id == ownerId)
                return (false, "You cannot share a trip with yourself.");

            var existingShare = await _context.TripShares
                .FirstOrDefaultAsync(s => s.TripId == request.TripId && s.SharedWithUserId == sharedWithUser.Id);

            if (existingShare != null)
                return (false, "This trip is already shared with the user.");

            if (!Enum.TryParse<AccessLevel>(request.AccessLevel, true, out var accessLevel))
                return (false, "Invalid access level.");

            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.Id == request.TripId && t.UserId == ownerId);

            if (trip == null)
                return (false, "Trip not found or you do not have permission to share it.");

            var share = new TripShare
            {
                TripId = request.TripId,
                OwnerId = ownerId,
                SharedWithUserId = sharedWithUser.Id,
                AccessLevel = accessLevel
            };

            _context.TripShares.Add(share);
            await _context.SaveChangesAsync();

            return (true, null);
        }


        public async Task<List<SharedTripDto>> GetTripsSharedWithUserAsync(int userId)
        {
            var sharedTrips = await _context.TripShares
                .Where(s => s.SharedWithUserId == userId)
                .Include(s => s.Trip)
                .Select(s => new SharedTripDto
                {
                    TripId = s.TripId,
                    AccessLevel = s.AccessLevel,
                    Title = s.Trip.Title,
                    Destination = s.Trip.Destination,
                    StartDate = s.Trip.StartDate,
                    EndDate = s.Trip.EndDate,
                    Description = s.Trip.Description,
                    TravelMode = s.Trip.TravelMode,
                    Budget = s.Trip.Budget,
                    Notes = s.Trip.Notes,
                    Image = s.Trip.Image,
                    Duration = s.Trip.Duration,
                    BestTime = s.Trip.BestTime,
                    Essentials = s.Trip.Essentials ?? new List<string>(),
                    TouristSpots = s.Trip.TouristSpots ?? new List<string>(),
                })
                .ToListAsync();

            return sharedTrips;
        }
    }
}
