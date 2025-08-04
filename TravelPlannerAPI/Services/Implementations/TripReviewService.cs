using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Services
{
    public class TripReviewService : ITripReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TripReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<TripReviewDto>> SearchReviewsByDestinationAsync(string destination)
        {
            return _unitOfWork.TripReview.SearchReviewsByDestinationAsync(destination);
        }
    }
}
