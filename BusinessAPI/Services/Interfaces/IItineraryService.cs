using BusinessAPI.Dtos;
using BusinessAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Interfaces
{
    public interface IItineraryService
    {
        Task<IEnumerable<ItineraryItem>> GetItineraryAsync(int tripId);
        Task<ItineraryItem> AddItineraryItemAsync(int tripId, ItineraryItemCreateDto dto);
        Task<bool> UpdateItineraryItemAsync(ItineraryItem item);
        Task<bool> DeleteItineraryItemAsync(int id);
        Task<IEnumerable<ItineraryItem>> GetSharedItineraryAsync(int tripId, int userId);
    }
}
