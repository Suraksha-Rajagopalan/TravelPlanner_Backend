using BusinessAPI.Dtos;
using BusinessAPI.Models;
using BusinessAPI.Models.Data;
using BusinessAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasAccessAsync(int tripId, int userId)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null) return false;

            if (trip.UserId == userId) return true;

            return await _context.TripShares
                .AnyAsync(s => s.TripId == tripId && s.SharedWithUserId == userId);
        }

        public async Task<bool> SubmitReviewAsync(ReviewDto dto, int userId)
        {
            if (!await HasAccessAsync(dto.TripId, userId))
                return false;

            var existing = await _context.Reviews
                .FirstOrDefaultAsync(r => r.TripId == dto.TripId && r.UserId == userId);

            if (existing != null)
                return false; // Already reviewed

            var review = new Review
            {
                TripId = dto.TripId,
                UserId = userId,
                Rating = dto.Rating,
                ReviewText = dto.Review ?? string.Empty
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ReviewDto> GetReviewAsync(int tripId, int userId)
        {
            if (!await HasAccessAsync(tripId, userId))
                return null;

            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.TripId == tripId && r.UserId == userId);

            if (review == null) return null;

            return new ReviewDto
            {
                TripId = review.TripId,
                UserId = review.UserId,
                Rating = review.Rating,
                Review = review.ReviewText
            };
        }
    }
}
