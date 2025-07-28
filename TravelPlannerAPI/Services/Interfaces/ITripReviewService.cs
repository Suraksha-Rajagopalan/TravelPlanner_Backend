using TravelPlannerAPI.Dtos;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface ITripReviewService
    {
        Task<List<TripReviewDto>> SearchReviewsByDestinationAsync(string destination);
    }
}
