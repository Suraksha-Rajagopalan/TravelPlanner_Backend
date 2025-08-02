using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using System.Threading.Tasks;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IReviewRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SubmitReviewAsync(ReviewDto dto, int userId)
        {
            // 1. Access check
            if (!await _repo.HasAccessAsync(dto.TripId, userId))
                return false;

            // 2. Already reviewed?
            var existing = await _repo.GetByTripAndUserAsync(dto.TripId, userId);
            if (existing != null)
                return false;

            // 3. Create
            var review = new ReviewModel
            {
                TripId = dto.TripId,
                UserId = userId,
                Rating = dto.Rating,
                ReviewText = dto.Review ?? string.Empty
            };

            await _repo.AddAsync(review);
            await _unitOfWork.CompleteAsync();      // commit via generic
            return true;
        }

        public async Task<ReviewDto?> GetReviewAsync(int tripId, int userId)
        {
            // 1. Access check
            if (!await _repo.HasAccessAsync(tripId, userId))
                return null;

            // 2. Fetch
            var review = await _repo.GetByTripAndUserAsync(tripId, userId);
            if (review == null) return null;

            // 3. Map to DTO
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
