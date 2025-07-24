using BusinessAPI.Dtos;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Interfaces
{
    public interface IReviewService
    {
        Task<bool> HasAccessAsync(int tripId, int userId);
        Task<bool> SubmitReviewAsync(ReviewDto dto, int userId);
        Task<ReviewDto> GetReviewAsync(int tripId, int userId);
    }
}
