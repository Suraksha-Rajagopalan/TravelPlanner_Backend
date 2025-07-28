using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IItineraryService
    {
        Task<IEnumerable<ItineraryItem>> GetItineraryItemsByTripIdAsync(int tripId);
        Task<ItineraryItem> GetItineraryItemByIdAsync(int id);
        Task<ItineraryItem> AddItineraryItemAsync(int tripId, ItineraryItemCreateDto dto);
        Task<bool> UpdateItineraryItemAsync(int id, ItineraryItemCreateDto dto);
        Task<bool> DeleteItineraryItemAsync(int id);
        Task<IEnumerable<ItineraryItem>> GetSharedItineraryAsync(int tripId, int userId);
    }
}
