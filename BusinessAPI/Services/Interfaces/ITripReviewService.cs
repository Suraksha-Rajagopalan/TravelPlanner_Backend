using BusinessAPI.Dtos;

namespace BusinessAPI.Services.Interfaces
{
    public interface ITripReviewService
    {
        Task<List<TripReviewDto>> SearchReviewsByDestinationAsync(string destination);
    }
}
