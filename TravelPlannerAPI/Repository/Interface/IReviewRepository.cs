using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        /// <summary>
        /// Can this user access (own or be shared) the given trip?
        /// </summary>
        Task<bool> HasAccessAsync(int tripId, int userId);

        /// <summary>
        /// Fetch the single review by trip & user (or null).
        /// </summary>
        Task<Review> GetByTripAndUserAsync(int tripId, int userId);
    }
}
