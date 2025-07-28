using TravelPlannerAPI.Dtos;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IReviewService
    {
        /// <summary>Submit a new review (returns false if no access or already reviewed).</summary>
        Task<bool> SubmitReviewAsync(ReviewDto dto, int userId);

        /// <summary>Get an existing review (null if none or no access).</summary>
        Task<ReviewDto> GetReviewAsync(int tripId, int userId);
    }
}
