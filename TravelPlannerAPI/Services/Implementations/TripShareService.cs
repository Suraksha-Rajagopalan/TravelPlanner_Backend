using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Models.Enums;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Services.Implementations
{
    public class TripShareService : ITripShareService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TripShareService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool Success, string ErrorMessage)> ShareTripAsync(
            int ownerId,
            TripShareRequestDto request)
        {
            // Lookup user by email
            var sharedWithUser = await _unitOfWork.TripShares.GetUserByEmailAsync(request.SharedWithEmail);

            if (sharedWithUser == null)
                return (false, "User to share with not found.");

            // Prevent self‑share
            if (sharedWithUser.Id == ownerId)
                return (false, "You cannot share a trip with yourself.");

            // Validate access level
            if (!Enum.TryParse<AccessLevel>(
                    request.AccessLevel,
                    ignoreCase: true,
                    out var accessLevel))
            {
                return (false, "Invalid access level.");
            }

            // Ensure trip belongs to owner
            var trip = await _unitOfWork.TripShares.GetOwnedTripAsync(request.TripId, ownerId);
            if (trip == null)
                return (false, "Trip not found or you do not own it.");

            // Prevent duplicate share
            var exists = await _unitOfWork.TripShares.GetByTripAndUserAsync(request.TripId, sharedWithUser.Id);
            if (exists != null)
                return (false, "This trip is already shared with the user.");

            // Create & persist
            var share = new TripShareModel
            {
                TripId = request.TripId,
                OwnerId = ownerId,
                SharedWithUserId = sharedWithUser.Id,
                AccessLevel = accessLevel
            };

            await _unitOfWork.TripShares.AddAsync(share);
            await _unitOfWork.CompleteAsync();
            return (true, "");
        }

        public async Task<IEnumerable<SharedTripDto>> GetTripsSharedWithUserAsync(int userId)
        {
            var shares = await _unitOfWork.TripShares.GetSharesForUserAsync(userId);

            return shares.Select(s => new SharedTripDto
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
                BestTime = s.Trip.BestTime ?? String.Empty,
                Essentials = s.Trip.Essentials ?? new List<string>(),
                TouristSpots = s.Trip.TouristSpots ?? new List<string>()
            }).ToList();
        }
    }
}
