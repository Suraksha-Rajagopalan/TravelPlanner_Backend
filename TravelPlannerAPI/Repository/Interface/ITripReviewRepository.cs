using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface ITripReviewRepository
    {
        Task<List<TripReviewDto>> SearchReviewsByDestinationAsync(string destination);
    }
}
