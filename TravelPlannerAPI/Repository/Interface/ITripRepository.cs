using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Helpers;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        Task<IEnumerable<Trip>> GetByUserAsync(int userId);
        Task<Trip> GetByIdWithIncludesAsync(int id);
        Task<PaginatedResult<Trip>> GetPaginatedTripsAsync(PaginationParamsDto pagination);
    }
}
