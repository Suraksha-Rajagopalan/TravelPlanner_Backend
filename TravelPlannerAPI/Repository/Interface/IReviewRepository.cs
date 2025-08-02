using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IReviewRepository : IGenericRepository<ReviewModel>
    {
        /// <summary>
        /// Can this user access (own or be shared) the given trip?
        /// </summary>
        Task<bool> HasAccessAsync(int tripId, int userId);

        /// <summary>
        /// Fetch the single review by trip & user (or null).
        /// </summary>
        Task<ReviewModel?> GetByTripAndUserAsync(int tripId, int userId);
    }
}
