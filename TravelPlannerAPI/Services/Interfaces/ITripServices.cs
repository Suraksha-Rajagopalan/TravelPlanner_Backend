using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface ITripService
    {
        Task<IEnumerable<Trip>> GetTripsAsync(int userId);
        Task<Trip> GetTripByIdAsync(int id, int userId);
        Task<Trip> CreateTripAsync(TripCreateDto dto, int userId);
        Task<bool> UpdateTripAsync(TripUpdateDto dto, int userId);
        Task<bool> DeleteTripAsync(int id, int userId);
    }
}
