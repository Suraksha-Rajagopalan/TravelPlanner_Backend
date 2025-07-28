using TravelPlannerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interfaces
{
    public interface IItineraryRepository
    {
        Task<IEnumerable<ItineraryItem>> GetByTripIdAsync(int tripId);
        Task<ItineraryItem> GetByIdAsync(int id);
        Task AddAsync(ItineraryItem item);
        Task UpdateAsync(ItineraryItem item);
        Task DeleteAsync(ItineraryItem item);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<ItineraryItem>> GetSharedItineraryAsync(int tripId, int userId);
    }
}
