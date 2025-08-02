using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IItineraryRepository : IGenericRepository<ItineraryItemsModel>
    {
        Task<IEnumerable<ItineraryItemsModel>> GetByTripIdAsync(int tripId);
        Task<ItineraryItemsModel?> GetByIdAsync(int id);
        Task newAddAsync(ItineraryItemsModel item);
        Task UpdateAsync(ItineraryItemsModel item);
        Task DeleteAsync(ItineraryItemsModel item);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<ItineraryItemsModel>> GetSharedItineraryAsync(int tripId, int userId);
    }
}
