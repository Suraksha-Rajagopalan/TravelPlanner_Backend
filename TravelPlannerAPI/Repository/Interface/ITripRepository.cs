using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Helpers;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface ITripRepository : IGenericRepository<TripModel>
    {
        Task<IEnumerable<TripModel>> GetByUserAsync(int userId);
        Task<TripModel?> GetByIdWithIncludesAsync(int id);
        Task<PaginatedResult<TripModel>> GetPaginatedTripsAsync(PaginationParamsDto pagination, int userId);
    }
}
