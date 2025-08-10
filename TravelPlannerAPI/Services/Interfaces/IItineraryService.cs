using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IItineraryService
    {
        Task<IEnumerable<ItineraryItemsModel>> GetItineraryItemsByTripIdAsync(int tripId);
        Task<ItineraryItemsModel?> GetItineraryItemByIdAsync(int id);
        Task<ItineraryItemsModel?> AddItineraryItemAsync(int tripId, ItineraryItemCreateDto dto, int userId);
        Task<bool> UpdateItineraryItemAsync(int id, ItineraryItemCreateDto dto, int userId);
        Task<bool> DeleteItineraryItemAsync(int id, int userId);
        Task<IEnumerable<ItineraryItemsModel>> GetSharedItineraryAsync(int tripId, int userId);
    }
}
