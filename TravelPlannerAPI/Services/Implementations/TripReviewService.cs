using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;

namespace TravelPlannerAPI.Services
{
    public class TripReviewService : ITripReviewService
    {
        private readonly ITripReviewRepository _tripreviewRepository;

        public TripReviewService(ITripReviewRepository tripreviewRepository)
        {
            _tripreviewRepository = tripreviewRepository;
        }

        public Task<List<TripReviewDto>> SearchReviewsByDestinationAsync(string destination)
        {
            return _tripreviewRepository.SearchReviewsByDestinationAsync(destination);
        }
    }
}
