using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Helpers;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface ITripService
    {
        Task<IEnumerable<TripModel>> GetTripsAsync(int userId);
        Task<PaginatedResult<TripDto>> GetPaginatedTripsAsync(PaginationParamsDto pagination);
        Task<TripModel?> GetTripByIdAsync(int id, int userId);
        Task<TripModel> CreateTripAsync(TripCreateDto dto, int userId);
        Task<bool> UpdateTripAsync(TripUpdateDto dto, int userId);
        Task<bool> DeleteTripAsync(int id, int userId);
    }
}
